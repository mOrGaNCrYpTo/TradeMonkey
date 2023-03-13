using CryptoExchange.Net.CommonObjects;

using Skender.Stock.Indicators;

using TradeMonkey.DataCollector.Strategies;
using TradeMonkey.Trader.Services;
using TradeMonkey.Trader.Strategies;
using TradeMonkey.Trader.Value.Aggregate;

public class FiveMinuteScalper : BaseStrategy
{
    private ILogger<FiveMinuteScalper> _logger;

    [InjectService]
    public TAIndicatorManager IndicatorManager { get; private set; }

    [InjectService]
    public KucoinAccountSvc KucoinAccountSvc { get; private set; }

    public FiveMinuteScalper(Symbol symbol, TAIndicatorManager indicatorManager, KucoinAccountSvc kucoinAccountSvc,
        ILogger<FiveMinuteScalper> logger)
    {
        IndicatorManager = indicatorManager ?? throw new ArgumentNullException(nameof(indicatorManager));
        KucoinAccountSvc = kucoinAccountSvc ?? throw new ArgumentNullException(nameof(kucoinAccountSvc));

        TradingCalculators = new TradingCalculators(KucoinAccountSvc);

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ExecuteAsync(IEnumerable<QuoteDto> history, CancellationToken cancellationToken = default)
    {
        var quotes = history.ToList();
        var tradingSignal = await GetTradingSignalAsync(quotes);

        switch (tradingSignal)
        {
            case TradingSignal.GoLong:
                var (stopLoss, takeProfit) = await GetStopLossTakeProfitPricesAsync(quotes);

                var orderSide = OrderSide.Buy;
                var quantity = GetQuantityToBuy((decimal)stopLoss, _riskPercent);

                var order = new MarketOrder(_symbol, orderSide, quantity);
                var result = await _symbol.Client.PostOrderAsync(order, cancellationToken);

                if (result.Success)
                {
                    _logger.LogInformation($"Buy order for {quantity} {_symbol.BaseCurrency} placed at {result.Data.OrderPrice}");

                    var filledQuantity = result.Data.QuantityFilled;
                    var stopLossOrder = new StopLossOrder(_symbol, OrderSide.Sell, filledQuantity, stopLoss);
                    var takeProfitOrder = new TakeProfitOrder(_symbol, OrderSide.Sell, filledQuantity, takeProfit);

                    var tasks = new List<Task<OrderPostResponse>>();

                    tasks.Add(_symbol.Client.PostOrderAsync(stopLossOrder, cancellationToken));
                    tasks.Add(_symbol.Client.PostOrderAsync(takeProfitOrder, cancellationToken));

                    var orders = await Task.WhenAll(tasks);

                    if (orders.All(o => o.Success))
                    {
                        _logger.LogInformation($"Stop loss order for {filledQuantity} {_symbol.BaseCurrency} placed at {stopLoss}");
                        _logger.LogInformation($"Take profit order for {filledQuantity} {_symbol.BaseCurrency} placed at {takeProfit}");
                    }
                    else
                    {
                        _logger.LogError($"Error placing stop loss or take profit order for {filledQuantity} {_symbol.BaseCurrency}");
                    }
                }
                else
                {
                    _logger.LogError($"Error placing buy order for {quantity} {_symbol.BaseCurrency}");
                }

                break;

            case TradingSignal.GoShort:
                // Similar logic for opening a short position goes here
                break;

            case TradingSignal.None:
            default:
                break;
        }
    }

    public async Task<(decimal?, decimal?)> GetStopLossTakeProfitPricesAsync(IEnumerable<QuoteDto> history)
    {
        var quotes = history.ToList();
        var price = quotes.Last().Close;
        var atr = IndicatorManager.GetAtr(quotes, 14);

        var stopLoss = price - (_stopLossMultiplier * atr);
        var takeProfit = price + (_rewardPercent * atr);

        return (stopLoss, takeProfit);
    }

    public Task<decimal> GetExitPriceAsync(IEnumerable<QuoteDto> history, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetPositionSizeAsync(IEnumerable<QuoteDto> history, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TimeSpan> GetDurationAsync(IEnumerable<QuoteDto> history, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private async Task<TradingSignal> GetTradingSignalAsync(List<QuoteDto> quotes)
    {
        // Get the last quote and the previous quote
        var lastQuote = quotes.Last();
        var prevQuote = quotes[^2];

        // Calculate the 20-period exponential moving average
        IEnumerable<EmaResult> ema20 = Indicator.GetEma(quotes, 20);

        // Calculate the MACD values
        MacdResult macd = Indicator.GetMacd(quotes, _smaFastPeriods, _smaSlowPeriods, _si);

        if (macd.Macd == null || macd.Signal == null)
        {
            return TradingSignal.None;
        }

        if (macd.Macd != 50)
        {
            // Check if the MACD signal crossed above the MACD line
            var macdCrossedAbove = macd.Signal.Value > macd.Macd.Value
                && macd.Signal.Value < macd.Macd.Value
                && macd.Signal.Value > macd.Signal.Value;

            // Check if the MACD signal crossed below the MACD line
            if (!macdCrossedAbove)
            {
                var macdCrossedBelow = macd.Signal.Value < macd.Macd.Value
                    && macd.Signal.Value > macd.Macd.Value
                    && macd.Signal.Value < macd.Signal.Value;
            }
        }

        // Check if the last quote is above the 20-period EMA
        var aboveEma20 = lastQuote.Close > ema20.Last().Ema;

        // Check if the last quote is below the 20-period EMA
        var belowEma20 = lastQuote.Close < ema20.Last().Ema;

        if (macdCrossedAbove && aboveEma20)
        {
            return TradingSignal.GoLong;
        }
        else if (macdCrossedBelow && belowEma20)
        {
            return TradingSignal.GoShort;
        }
        else
        {
            return TradingSignal.None;
        }
    }

    private bool CheckForHiddenBullishDivergence(List<QuoteDto> quotes, decimal rsi)
    {
        // Check if there is a lower low in the price and a higher low in the RSI
        for (int i = quotes.Count - 2; i >= 0; i--)
        {
            if (quotes[i].Low < quotes.Last().Low && IndicatorManager.GetRsi(quotes.Skip(i).Take(_rsiPeriods + 1), _rsiPeriods).First() > rsi)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckForHiddenBearishDivergence(IEnumerable<QuoteDto> history, decimal rsi)
    {
        var quotes = history.ToList();
        var oscillator = IndicatorManager.GetMomentumOscillator(quotes, _oscillatorPeriod);

        // Find the peaks of the oscillator
        List<PeakPoint> peakPoints = oscillator.GetPeakPoints(oscillator);

        if (peakPoints.Count < 2)
            return false;

        // Determine the most recent peak and the one before it
        var currentPeak = peakPoints.Last();
        var previousPeak = peakPoints[peakPoints.Count - 2];

        // Find the lows between the two peaks
        var lows = quotes.GetRange(previousPeak.Index, currentPeak.Index - previousPeak.Index + 1).Select(q => q.Low).ToList();
        var lowPoints = lows.GetLowPoints();

        // Determine if the RSI has formed a hidden bearish divergence pattern
        var previousRsiValue = IndicatorManager.GetRsi(quotes.GetRange(previousPeak.Index - _rsiPeriods + 1, _rsiPeriods + 1), _rsiPeriods).First();
        var rsiIsDivergent = rsi < previousRsiValue;

        // Determine if the oscillator has formed a hidden bearish divergence pattern
        var previousOscillatorValue = oscillator[previousPeak.Index];
        var oscillatorIsDivergent = previousOscillatorValue < oscillator[currentPeak.Index];

        // Check if there is a bullish divergence between the RSI and the lows
        var rsiDivergence = IndicatorManager.CheckForBullishDivergence(quotes, lowPoints, _rsiPeriods);

        return rsiIsDivergent && oscillatorIsDivergent && rsiDivergence;
    }
}