using TradeMonkey.Trader.Interfaces;

namespace TradeMonkey.Trader
{
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

            // If the ROC value is above the threshold, indicating a fast upward movement, increase
            // the take profit multiplier
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
            var x = quotes.FirstOrDefault();

            // Use a weighted points based system
            int goLongPoints = 0;
            int goShortPoints = 0;
            int noTradePoints = 0;

            //You can consider creating separate rule sets for long and short trades.
            //This can help you to better organize your rules and make it easier to manage them.
            var tradingRules = new List<ITradingRule>
            {
                // ...
            };

            var longTradingRules = new List<ITradingRule> { /*...*/ };
            var shortTradingRules = new List<ITradingRule> { /*...*/ };

            // Check if all long rules are met, and if not, check if all short rules are met. If
            // neither set of rules is entirely met, return TradingSignal.None.
            foreach (var rule in tradingRules)
            {
                //if (await rule.EvaluateRuleSetAsync(quotes))
                //{
                //    return rule.Signal;
                //}
            }

            return TradingSignal.None;

            // Get the last quote and the previous quote
            var lastQuote = quotes.Last();
            var prevQuote = quotes[^2];

            Tuple<bool, bool, bool, bool> crossOverSignals =
                await TAIndicatorManager.GetEmaSmmaCrossoverSignals(quotes, lastQuote, Ema50Periods, Ema100Periods,
                Smma21Periods, Smma50Periods, Smma200Periods);

            // ATR
            decimal atr = TAIndicatorManager.GetAtr(quotes, 14);

            // Assign points for each crossover signal and multiply the points awarded the ATR value
            // or reduce Assign points for each crossover signal, adjusted for ATR
            goLongPoints =
                ((crossOverSignals.Item1 ? 1 : 0) * (int)Math.Floor(atr)) + ((crossOverSignals.Item3 ? 1 : 0) * (int)Math.Floor(atr * 0.5m));

            goShortPoints =
                ((crossOverSignals.Item2 ? 1 : 0) * (int)Math.Floor(atr)) + ((crossOverSignals.Item4 ? 1 : 0) * (int)Math.Floor(atr * 0.5m));

            // Unused: Stochastic oscillator
            var stochasticOscillator = Indicator.GetStoch(quotes, StochasticKLength, StochasticKSmoothing, StochasticDSmoothing);

            // Unused: Bollinger Bands
            var bollingerBands = Indicator.GetBollingerBands(quotes, BollingerBandsPeriods, BollingerBandsStdDev);

            // Unused: Use support and resistance (use GetSupportLevel and GetResistanceLevel methods)
            var support = TAIndicatorManager.GetSupportLevel(quotes);
            var resistance = TAIndicatorManager.GetResistanceLevel(quotes);

            // Rule 15: Only trade with the trend
            var isUpTrend = TAIndicatorManager
                                .IsUptrend(quotes: quotes,
                                           smaPeriodShort: SmaFastPeriods,
                                           smaPeriodLong: SmaSlowPeriods);

            // Not yet utilized: Trade engulfing candles and 3 line strikes
            var isEngulfingCandle = TAIndicatorManager.IsEngulfingCandle(prevQuote, lastQuote);
            var isThreeLineStrike = TAIndicatorManager.IsThreeLineStrike(quotes);

            // Not yet utilized: Trade at the close of the first candle that crosses the 200 SMMA
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

            IEnumerable<EmaResult> ema20 = Indicator.GetEma(quotes, 20);

            // Check if the last quote is above the 20-period EMA
            var aboveEma20 = lastQuote.Close > (decimal)ema20.Last().Ema;

            // Check if the last quote is below the 20-period EMA
            var belowEma20 = lastQuote.Close < (decimal)ema20.Last().Ema;

            TradingSignal signal = TradingSignal.None;

            //if (goLongPoints > goShortPoints && goLongPoints > noTradePoints && isUpTrend)
            //{
            //    signal = TradingSignal.GoLong;
            //}
            //else if (goShortPoints > goLongPoints && goShortPoints > noTradePoints && !isUpTrend)
            //{
            //    signal = TradingSignal.GoShort;
            //}
            //else if (macdCrossedAbove && aboveEma20)
            //{
            //    signal = TradingSignal.GoLong;
            //}
            //else if (macdCrossedBelow && belowEma20)
            //{
            //    signal = TradingSignal.GoShort;
            //}

            var rsi = TAIndicatorManager.GetPreviousRsi(quotes, RsiPeriods);
            var isRsiValidForPosition = TAIndicatorManager.IsRsiValidForPosition(rsi, signal);

            return isRsiValidForPosition ? signal : TradingSignal.None;
        }
    }
}