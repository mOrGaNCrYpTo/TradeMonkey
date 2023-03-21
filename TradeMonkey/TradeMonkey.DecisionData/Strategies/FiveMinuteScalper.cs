using CryptoExchange.Net.CommonObjects;

namespace TradeMonkey.Trader
{
    // Modularize the trading rules to make the code more maintainable and easier to read. This would also allow you to experiment with different combinations of rules by simply enabling or disabling them withinthe GetTradingSignalAsync method.

    // Implement an adaptive mechanism for adjusting parameters: You could implement a simple adaptive mechanism that adjusts certain parameters based on the recent performance of the strategy.For example, you could modify the rocThreshold based on how well the strategy has been performing lately.A
    // more advanced approach would be to use machine learning techniques to optimize the parameters of the strategy based on historical data. Optimize the order execution process: You can optimize the order execution process by asynchronously executing the orders and monitoring their progress. This
    // can help reduce the time spent waiting for orders to be placed and improve the strategy's responsiveness to market movements.

    // Monitor and adapt to market conditions: Consider adding functionality to monitor market conditions and adjust the strategy accordingly. For example, you could track the overall market trend and only trade when the market is trending in the direction of the strategy. Risk management and
    // position sizing: Implement dynamic risk management and position sizing algorithms to further optimize the strategy's performance. This could include adjusting the position size based on the current account balance and the strategy's recent performance.

    public class FiveMinuteScalper : BaseStrategy
    {
        [InjectService]
        public KucoinAccountSvc KucoinAccountSvc { get; private set; }

        [InjectService]
        public KucoinOrderSvc OrderService { get; private set; }

        [InjectService]
        public TokenMetricsSvc TokenMetricsSvc { get; private set; }

        [InjectService]
        public TradingCalculators TradingCalculators { get; private set; }

        // Additional properties for adaptive parameters
        public int StopLossMultiplier { get; set; }

        public int RewardPercent { get; set; }

        public FiveMinuteScalper(
        KucoinAccountSvc kucoinAccountSvc,
        KucoinOrderSvc kucoinOrderSvc,
        TokenMetricsSvc tokenMetricsSvc,
        TradingCalculators tradingCalculators,
        Symbol symbol,
        ILogger<FiveMinuteScalper> logger,
        int stopLossMultiplier = 2,
        int rewardPercent = 2) : base(tradingCalculators, symbol, logger)
        {
            KucoinAccountSvc = kucoinAccountSvc ?? throw new ArgumentNullException(nameof(kucoinAccountSvc));
            TokenMetricsSvc = tokenMetricsSvc ?? throw new ArgumentNullException(nameof(tokenMetricsSvc));
            OrderService = kucoinOrderSvc ?? throw new ArgumentNullException(nameof(kucoinOrderSvc));
            TradingCalculators = tradingCalculators ?? throw new ArgumentNullException(nameof(tradingCalculators));
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));

            StopLossMultiplier = stopLossMultiplier;
            RewardPercent = rewardPercent;
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
                    var quantity = await TradingCalculators.GetQuantityToBuy(EntryPrice, StopLossPrice, RiskPercent, Symbol.Name, ct);
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

                        await Task.WhenAll(tasks);
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

            decimal stopLossMultiplier = 2; // Customize the multiplier based on your risk tolerance
            decimal takeProfitMultiplier = 3; // Customize the multiplier based on your desired reward ratio

            int rocPeriod = 9; // Customize the ROC period as needed
            decimal rocThreshold = 2.0M; // Customize the ROC threshold to determine fast movement

            var roc = TAIndicatorManager.GetRateOfChange(quotes, rocPeriod);

            // If the ROC value is above the threshold, indicating a fast upward movement, increase the take profit multiplier
            if (roc > rocThreshold)
            {
                takeProfitMultiplier = 4; // Customize the increased multiplier based on your desired reward ratio
            }

            var stopLoss = price - (stopLossMultiplier * atr);
            var takeProfit = price + (takeProfitMultiplier * atr);

            return (stopLoss, takeProfit);
        }

        private async Task<TradingSignal> GetTradingSignalAsync(List<QuoteDto> quotes)
        {
            int goLongPoints = 0;
            int goShortPoints = 0;
            int noTradePoints = 0;

            // Get the last quote and the previous quote
            var lastQuote = quotes.Last();
            var prevQuote = quotes[^2];

            // Rule 5: Calculate EMA(50) and EMA(100) Calculate the 20-period exponential moving average
            IEnumerable<EmaResult> ema20 = Indicator.GetEma(quotes, 20);
            IEnumerable<EmaResult> ema50 = Indicator.GetEma(quotes, Ema50Periods);
            IEnumerable<EmaResult> ema100 = Indicator.GetEma(quotes, Ema100Periods);

            // Rule 16, 17, 18: SMMA (21, 50, 200) and position rules
            IEnumerable<SmaResult> smma21 = Indicator.GetSma(quotes, Smma21Periods);
            IEnumerable<SmaResult> smma50 = Indicator.GetSma(quotes, Smma50Periods);
            IEnumerable<SmaResult> smma200 = Indicator.GetSma(quotes, Smma200Periods);

            decimal atr = TAIndicatorManager.GetAtr(quotes, 14);

            // Rule 11: Stochastic oscillator
            var stochasticOscillator = Indicator.GetStoch(quotes, StochasticKLength, StochasticKSmoothing, StochasticDSmoothing);

            // Rule 12: Bollinger Bands
            var bollingerBands = Indicator.GetBollingerBands(quotes, BollingerBandsPeriods, BollingerBandsStdDev);

            //TokenMetricsResSuppDatum tokenMetricsResSuppDatum = new TokenMetricsResSuppDatum();

            // Rule 13, 14: Use support and resistance (use GetSupportLevel and GetResistanceLevel methods)
            var support = TAIndicatorManager.GetSupportLevel(quotes);
            var resistance = TAIndicatorManager.GetResistanceLevel(quotes);

            // Rule 15: Only trade with the trend
            var isUpTrend = TAIndicatorManager
                                .IsUptrend(quotes: quotes,
                                           smaPeriodShort: SmaFastPeriods,
                                           smaPeriodLong: SmaSlowPeriods);

            // Rule 19: Trade engulfing candles and 3 line strikes
            var isEngulfingCandle = TAIndicatorManager.IsEngulfingCandle(prevQuote, lastQuote);
            var isThreeLineStrike = TAIndicatorManager.IsThreeLineStrike(quotes);

            // Rule 23: Trade at the close of the first candle that crosses the 200 SMMA
            var isFirstSignal = TAIndicatorManager.IsFirstCandleCrossingSMA200(quotes);

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

            TradingSignal signal = TradingSignal.None;

            if (macdCrossedAbove && aboveEma20)
            {
                signal = TradingSignal.GoLong;
            }
            else if (macdCrossedBelow && belowEma20)
            {
                signal = TradingSignal.GoShort;
            }

            var rsi = TAIndicatorManager.GetPreviousRsi(quotes, RsiPeriods);
            var isRsiValidForPosition = TAIndicatorManager.IsRsiValidForPosition(rsi, signal);

            if (isRsiValidForPosition)
            {
                return signal;
            }
            else
            {
                return TradingSignal.None;
            }
        }

        //private Tuple<int, int, int, int, int> GetMovingAverages()
        //{
        //    var ema50 = Indicator.GetEma(quotes, Ema50Periods);
        //    var ema100 = Indicator.GetEma(quotes, Ema100Periods);

        // // Rule 16, 17, 18: SMMA (21, 50, 200) and position rules var smma21 = Indicator.GetSma(quotes, Smma21Periods).; var smma50 = Indicator.GetSma(quotes, Smma50Periods); var smma200 = Indicator.GetSma(quotes, Smma200Periods);

        //    return new Tuple<int, int, int, int, int> { ema50, ema100, S }
        //}
    }
}