namespace TradeMonkey.Trader
{
    public class FiveMinuteScalper : BaseStrategy
    {
        [InjectService]
        public KucoinAccountSvc KucoinAccountSvc { get; private set; }

        [InjectService]
        public KucoinOrderSvc OrderService { get; private set; }

        public FiveMinuteScalper(Symbol symbol, KucoinAccountSvc kucoinAccountSvc,
          KucoinOrderSvc kucoinOrderSvc, TradingCalculators tradingCalculators, ILogger<FiveMinuteScalper> logger)
                : base(tradingCalculators, symbol, logger)
        {
            KucoinAccountSvc = kucoinAccountSvc ?? throw new ArgumentNullException(nameof(kucoinAccountSvc));
            OrderService = kucoinOrderSvc ?? throw new ArgumentNullException(nameof(kucoinOrderSvc));
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));

            StopLossMultiplier = 2;
            RewardPercent = 2;
        }

        public async Task ExecuteAsync(IEnumerable<QuoteDto> history, CancellationToken ct = default)
        {
            var quotes = history.ToList();
            var tradingSignal = await GetTradingSignalAsync(quotes);

            switch (tradingSignal)
            {
                case TradingSignal.GoLong:
                    var (stopLoss, takeProfit) = await GetStopLossTakeProfitPricesAsync(quotes);
                    var orderSide = OrderSide.Buy;
                    var quantity = await Calculators.GetQuantityToBuy(EntryPrice, StopLossPrice, RiskPercent, Symbol.Name, ct);
                    var result = await OrderService.PostMarketOrderAsync(Symbol.Name, orderSide, quantity, ct);

                    if (result.Success)
                    {
                        Loggy.LogInformation($"Buy order for {quantity} {Symbol.Name} placed at {DateTime.Now}");

                        decimal filledQuantity = quantity;

                        var stopLossOrder =
                            OrderService.PostStopOrderAsync
                            (
                                symbol: Symbol.Name,
                                orderSide: OrderSide.Sell,
                                quantity: filledQuantity,
                                tradeType: TradeType.SpotTrade,
                                stopPrice: (decimal)stopLoss,
                                timeInForce: TimeInForce.GoodTillCanceled,
                                cancelAfter: TimeSpan.FromDays(5),
                                token: ct
                            );

                        var takeProfitOrder =
                            OrderService.PostStopOrderAsync
                            (
                                symbol: Symbol.Name,
                                orderSide: OrderSide.Sell,
                                quantity: filledQuantity,
                                tradeType: TradeType.SpotTrade,
                                stopPrice: (decimal)takeProfit,
                                timeInForce: TimeInForce.GoodTillCanceled,
                                cancelAfter: TimeSpan.FromDays(5),
                                token: ct
                            );

                        List<Task> tasks = new List<Task> { stopLossOrder, takeProfitOrder };

                        _ = await Task.WhenAll(tasks);
                    }
                    else
                    {
                        Loggy.LogError($"Error placing buy order for {quantity} {Symbol.Name}");
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
            var atr = TAIndicatorManager.GetAtr(quotes, 14);

            var stopLoss = price - (StopLossMultiplier * atr);
            var takeProfit = price + (RewardPercent * atr);

            return (stopLoss, takeProfit);
        }

        private async Task<TradingSignal> GetTradingSignalAsync(List<QuoteDto> quotes)
        {
            // Get the last quote and the previous quote
            var lastQuote = quotes.Last();
            var prevQuote = quotes[^2];

            // Calculate the 20-period exponential moving average
            IEnumerable<EmaResult> ema20 = Indicator.GetEma(quotes, 20);

            // Calculate the MACD values
            var macdResults = quotes.GetMacd(SmaFastPeriods, SmaSlowPeriods, SignalPeriods).ToList();
            var lastMacdResult = macdResults.Last();
            var prevMacdResult = macdResults[^2];

            bool macdCrossedAbove = false;
            bool macdCrossedBelow = false;

            if (lastMacdResult.Macd.HasValue && lastMacdResult.Signal.HasValue)
            {
                // Check if the MACD signal crossed above the MACD line
                macdCrossedAbove = prevMacdResult.Macd < prevMacdResult.Signal && lastMacdResult.Macd > lastMacdResult.Signal;

                // Check if the MACD signal crossed below the MACD line
                macdCrossedBelow = prevMacdResult.Macd > prevMacdResult.Signal && lastMacdResult.Macd < lastMacdResult.Signal;
            }

            // Check if the last quote is above the 20-period EMA
            var aboveEma20 = lastQuote.Close > (decimal)ema20.Last().Ema;

            // Check if the last quote is below the 20-period EMA
            var belowEma20 = lastQuote.Close < (decimal)ema20.Last().Ema;

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

        private bool CheckForHiddenBullishDivergence(List<IQuote> quotes)
        {
            return TAIndicatorManager.CheckForBullishDivergence(quotes, OscillatorPeriod);
        }

        private bool CheckForHiddenBearishDivergence(IEnumerable<QuoteDto> history, decimal rsi)
        {
            var quotes = history.ToList();
            var oscillator = TAIndicatorManager.GetMomentumOscillator(quotes, OscillatorPeriod);

            // Find the peaks of the oscillator
            List<PeakPoint> peakPoints = quotes.GetPeakPoints(oscillator);

            if (peakPoints.Count < 2)
                return false;

            // Determine the most recent peak and the one before it
            var currentPeak = peakPoints.Last();
            var previousPeak = peakPoints[peakPoints.Count - 2];

            // Find the lows between the two peaks
            var lows = quotes.Skip(previousPeak.Index).Take(currentPeak.Index - previousPeak.Index + 1).Select(q => q.Low).ToList();
            var lowPoints = TAIndicatorManager.FindLowPoints();

            // Determine if the RSI has formed a hidden bearish divergence pattern
            var previousRsiValue = TAIndicatorManager.GetCurrentRsi(quotes.GetRange(previousPeak.Index - RsiPeriods + 1, RsiPeriods + 1), _rsiPeriods).First();
            var rsiIsDivergent = rsi < previousRsiValue;

            // Determine if the oscillator has formed a hidden bearish divergence pattern
            var previousOscillatorValue = oscillator[previousPeak.Index];
            var oscillatorIsDivergent = previousOscillatorValue < oscillator[currentPeak.Index];

            // Check if there is a bullish divergence between the RSI and the lows
            var rsiDivergence = TAIndicatorManager.CheckForBullishDivergence(quotes, lowPoints, RsiPeriods);

            return rsiIsDivergent && oscillatorIsDivergent && rsiDivergence;
        }
    }
}