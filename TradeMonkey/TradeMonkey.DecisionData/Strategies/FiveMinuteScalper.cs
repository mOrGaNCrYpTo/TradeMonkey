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

        public FiveMinuteScalper(Symbol symbol, KucoinAccountSvc kucoinAccountSvc,
          KucoinOrderSvc kucoinOrderSvc, TokenMetricsSvc tokenMetricsSvc, TradingCalculators tradingCalculators,
          ILogger<FiveMinuteScalper> logger)
                : base(tradingCalculators, symbol, logger)
        {
            KucoinAccountSvc = kucoinAccountSvc ?? throw new ArgumentNullException(nameof(kucoinAccountSvc));
            TokenMetricsSvc = tokenMetricsSvc ?? throw new ArgumentNullException(nameof(tokenMetricsSvc));
            OrderService = kucoinOrderSvc ?? throw new ArgumentNullException(nameof(kucoinOrderSvc));
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            TradingCalculators = tradingCalculators ?? throw new ArgumentNullException(nameof(tradingCalculators));

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

            var stopLoss = price - (StopLossMultiplier * atr);
            var takeProfit = price + (RewardPercent * atr);

            return (stopLoss, takeProfit);
        }

        public bool CheckForHiddenBullishDivergence(List<IQuote> quotes)
        {
            return TAIndicatorManager.CheckForBullishDivergence(quotes, OscillatorPeriod);
        }

        public bool CheckForHiddenBearishDivergence(IEnumerable<QuoteDto> history, decimal rsi)
        {
            var quotes = history.ToList();
            var oscillator = TAIndicatorManager.GetMomentumOscillator(quotes, OscillatorPeriod);

            // Find the peaks of the oscillator
            List<PeakPoint> peakPoints = TAIndicatorManager.GetPeakPoints(oscillator);

            if (peakPoints.Count < 2)
                return false;

            // Determine the most recent peak and the one before it
            var currentPeak = peakPoints.Last();
            var previousPeak = peakPoints[peakPoints.Count - 2];

            // Find the lows between the two peaks
            var lows = quotes.Skip(previousPeak.Index).Take(currentPeak.Index - previousPeak.Index + 1).Select(q => q.Low).ToList();

            List<IQuote> iquotes = (List<IQuote>)history.OfType<IQuote>();
            List<int> lowPoints = TAIndicatorManager.FindLowPoints(iquotes);

            // Determine if the RSI has formed a hidden bearish divergence pattern
            var previousRsiValue = TAIndicatorManager
                .GetCurrentRsi(quotes.GetRange(previousPeak.Index - RsiPeriods + 1, RsiPeriods + 1), RsiPeriods);

            var rsiIsDivergent = rsi < previousRsiValue;

            // Determine if the oscillator has formed a hidden bearish divergence pattern
            decimal previousOscillatorValue = oscillator[previousPeak.Index];
            var oscillatorIsDivergent = previousOscillatorValue < oscillator[currentPeak.Index];

            // Check if there is a bullish divergence between the RSI and the lows
            iquotes = (List<IQuote>)quotes.OfType<IQuote>();
            var rsiDivergence = TAIndicatorManager.CheckForBullishDivergence(iquotes, RsiPeriods);

            return rsiIsDivergent && oscillatorIsDivergent && rsiDivergence;
        }

        private async Task<TradingSignal> GetTradingSignalAsync(List<QuoteDto> quotes)
        {
            // Get the last quote and the previous quote
            var lastQuote = quotes.Last();
            var prevQuote = quotes[^2];

            // Rule 5: Calculate EMA(50) and EMA(100)
            var ema50 = Indicator.GetEma(quotes, Ema50Periods);
            var ema100 = Indicator.GetEma(quotes, Ema100Periods);

            // Rule 16, 17, 18: SMMA (21, 50, 200) and position rules
            var smma21 = Indicator.GetSma(quotes, Smma21Periods);
            var smma50 = Indicator.GetSma(quotes, Smma50Periods);
            var smma200 = Indicator.GetSma(quotes, Smma200Periods);

            // Rule 11: Stochastic oscillator
            var stochasticOscillator = Indicator.GetStoch(quotes, StochasticKLength, StochasticKSmoothing, StochasticDSmoothing);

            // Rule 12: Bollinger Bands
            var bollingerBands = Indicator.GetBollingerBands(quotes, BollingerBandsPeriods, BollingerBandsStdDev);

            //TokenMetricsResSuppDatum tokenMetricsResSuppDatum = new TokenMetricsResSuppDatum();

            // Rule 13, 14: Use support and resistance (use GetSupportLevel and GetResistanceLevel methods)
            var support = TAIndicatorManager.GetSupportLevel(quotes);
            var resistance = TAIndicatorManager.GetResistanceLevel(quotes);

            // Rule 15: Only trade with the trend
            var isUpTrend = TAIndicatorManager.IsUptrend(quotes: quotes,
                                                         smaPeriodShort: SmaFastPeriods,
                                                         smaPeriodLong: SmaSlowPeriods);

            // Rule 19: Trade engulfing candles and 3 line strikes
            var isEngulfingCandle = TAIndicatorManager.IsEngulfingCandle(prevQuote, lastQuote);
            var isThreeLineStrike = TAIndicatorManager.IsThreeLineStrike(quotes);

            // Rule 23: Trade at the close of the first candle that crosses the 200 SMMA
            var isFirstSignal = TAIndicatorManager.IsFirstCandleCrossingSMA200(quotes);

            // Rule 6: Open a long position on bullish crossover (already implemented in the
            // original code)

            // Rule 9, 10: Use bullish and bearish divergences and ATR (already implemented in the
            // original code)

            // Rule 25: 1:2 risk to reward ratio (use RiskPercent and TakeProfitPercent properties)

            // Rule 26: If resistance is above then sell .08 and wait for resistance to sell the
            // rest (or wait for a divergence)

            // Rule 27: Only risk 1% of account balance per trade, with a take profit of 2% of
            // account balance

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

            TradingSignal signal = TradingSignal.None;

            if (macdCrossedAbove && aboveEma20)
            {
                signal = TradingSignal.GoLong;
            }
            else if (macdCrossedBelow && belowEma20)
            {
                signal = TradingSignal.GoShort;
            }

            var rsi = TAIndicatorManager.GetCurrentRsi(quotes, RsiPeriods);
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
    }
}