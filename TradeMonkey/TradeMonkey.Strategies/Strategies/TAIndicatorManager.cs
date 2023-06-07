namespace TradeMonkey.Trader.Strategies
{
    [RegisterService]
    public static class TAIndicatorManager
    {
        public static IEnumerable<MacdResult> GetMacd<TQuote>(
               this IEnumerable<TQuote> quotes,
               int fastPeriods = 12,
               int slowPeriods = 26,
               int signalPeriods = 9)
               where TQuote : IQuote => quotes
           .ToTupleCollection(CandlePart.Close)
           .CalcMacd(fastPeriods, slowPeriods, signalPeriods);

        internal static List<MacdResult> CalcMacd(
       this List<(DateTime, double)> tpList,
       int fastPeriods,
       int slowPeriods,
       int signalPeriods)
        {
            // check parameter arguments
            ValidateMacd(fastPeriods, slowPeriods, signalPeriods);

            // initialize
            List<EmaResult> emaFast = tpList.CalcEma(fastPeriods);
            List<EmaResult> emaSlow = tpList.CalcEma(slowPeriods);

            int length = tpList.Count;
            List<(DateTime, double)> emaDiff = new();
            List<MacdResult> results = new(length);

            // roll through quotes
            for (int i = 0; i < length; i++)
            {
                (DateTime date, double _) = tpList[i];
                EmaResult df = emaFast[i];
                EmaResult ds = emaSlow[i];

                MacdResult r = new(date)
                {
                    FastEma = df.Ema,
                    SlowEma = ds.Ema
                };
                results.Add(r);

                if (i >= slowPeriods - 1)
                {
                    double macd = (df.Ema - ds.Ema).Null2NaN();
                    r.Macd = macd.NaN2Null();

                    // temp data for interim EMA of macd
                    (DateTime, double) diff = (date, macd);

                    emaDiff.Add(diff);
                }
            }

            // add signal and histogram to result
            List<EmaResult> emaSignal = CalcEma(emaDiff, signalPeriods);

            for (int d = slowPeriods - 1; d < length; d++)
            {
                MacdResult r = results[d];
                EmaResult ds = emaSignal[d + 1 - slowPeriods];

                r.Signal = ds.Ema.NaN2Null();
                r.Histogram = (r.Macd - r.Signal).NaN2Null();
            }

            return results;
        }

        // parameter validation
        private static void ValidateMacd(
            int fastPeriods,
            int slowPeriods,
            int signalPeriods)
        {
            // check parameter arguments
            if (fastPeriods <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fastPeriods), fastPeriods,
                    "Fast periods must be greater than 0 for MACD.");
            }

            if (signalPeriods < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(signalPeriods), signalPeriods,
                    "Signal periods must be greater than or equal to 0 for MACD.");
            }

            if (slowPeriods <= fastPeriods)
            {
                throw new ArgumentOutOfRangeException(nameof(slowPeriods), slowPeriods,
                    "Slow periods must be greater than the fast period for MACD.");
            }
        }
        public static List<decimal> GetMomentumOscillator(IEnumerable<QuoteDto> quotes, int period)
        {
            var momentum = CalculateMomentum(quotes, period);
            var sma = CalculateSimpleMovingAverage(momentum, period);
            return CalculateMomentumOscillator(momentum, sma, period);
        }

        private static List<decimal> CalculateMomentum(IEnumerable<QuoteDto> quotes, int period)
        {
            return quotes.Select((quote, index) => index < period ? 0 : quote.Close - quotes.ElementAt(index - period).Close).ToList();
        }

        private static List<decimal> CalculateSimpleMovingAverage(IEnumerable<decimal> momentum, int period)
        {
            return momentum.Select((value, index) => index < period - 1 ? 0 : momentum.Skip(index - period + 1).Take(period).Sum() / period).ToList();
        }

        private static List<decimal> CalculateMomentumOscillator(IEnumerable<decimal> momentum, IEnumerable<decimal> sma, int period)
        {
            int thresholdIndex = (period * 2) - 2;
            return momentum.Zip(sma, (currentMomentum, currentSma) => (index: momentum.IndexOf(currentMomentum), value: currentMomentum - currentSma))
                           .Select(item => item.index < thresholdIndex ? 0 : item.value)
                           .ToList();
        }

        public static decimal GetRateOfChange(List<QuoteDto> quotes, int period)
        {
            if (quotes == null || quotes.Count == 0)
            {
                throw new ArgumentException("The quotes list cannot be null or empty.");
            }

            if (period < 1)
            {
                throw new ArgumentException("The period must be greater than or equal to 1.");
            }

            if (quotes.Count < period + 1)
            {
                throw new ArgumentException($"The quotes list must have at least {period + 1} items for the specified period.");
            }

            decimal currentPrice = quotes.Last().Close;
            decimal previousPrice = quotes[quotes.Count - period - 1].Close;

            if (previousPrice == 0)
            {
                throw new InvalidOperationException("The previous price cannot be zero.");
            }

            decimal roc = (currentPrice - previousPrice) / previousPrice * 100;

            return roc;
        }

        public static List<PeakPoint> GetPeakPoints(this IEnumerable<decimal> values)
        {
            var peaks = new List<PeakPoint>();
            var valuesList = values.ToList();

            for (int i = 1; i < valuesList.Count - 1; i++)
            {
                if (valuesList[i] > valuesList[i - 1] && valuesList[i] > valuesList[i + 1])
                {
                    peaks.Add(new PeakPoint { Value = valuesList[i], Index = i });
                }
            }

            return peaks;
        }

        public static decimal GetPreviousRsi<TQuote>(this IEnumerable<TQuote> quotes, int periods) where TQuote : IQuote
        {
            var rsi = quotes.GetRsi(periods).LastOrDefault()?.Rsi ?? default;

            return (decimal)rsi;
        }

        public static IEnumerable<decimal> GetAllRsi(IEnumerable<IQuote> quotes, int periods)
        {
            return quotes.GetRsi(periods)
                    .Select(rsiResult => (decimal)rsiResult.Rsi);
        }

        public static List<int> FindHighPoints(List<IQuote> quotes)
        {
            List<int> highPoints = new List<int>();

            for (int i = 1; i < quotes.Count - 1; i++)
            {
                if (IsHighPoint(quotes, i))
                {
                    highPoints.Add(i);
                }
            }

            return highPoints;
        }

        public static List<int> FindLowPoints(List<IQuote> quotes)
        {
            List<int> lowPoints = new List<int>();

            for (int i = 1; i < quotes.Count - 1; i++)
            {
                if (IsLowPoint(quotes, i))
                {
                    lowPoints.Add(i);
                }
            }

            return lowPoints;
        }

        private static bool IsHighPoint(List<IQuote> quotes, int index)
        {
            return quotes[index].High > quotes[index - 1].High && quotes[index].High > quotes[index + 1].High;
        }

        private static bool IsLowPoint(List<IQuote> quotes, int index)
        {
            return quotes[index].Low < quotes[index - 1].Low && quotes[index].Low < quotes[index + 1].Low;
        }

        public static decimal GetAtr(IEnumerable<IQuote> quotes, int period)
        {
            var candles = quotes.Select(q => new Quote
            {
                Date = q.Date,
                Open = q.Open,
                High = q.High,
                Low = q.Low,
                Close = q.Close,
                Volume = q.Volume
            }).ToList();

            var atrValues = Indicator.GetAtr(candles, period);

            return atrValues.LastOrDefault()?.Atr ?? 0m;
        }

        public static IEnumerable<RollingPivotsResult> GetRollingPivots(IEnumerable<IQuote> quotes, int windowPeriods, int offsetPeriods, PivotPointType pointType = PivotPointType.Standard)
        {
            var candles = quotes.Select(q => new Candle(q.Date, q.Open, q.High, q.Low, q.Close, q.Volume)).ToList();

            return candles.GetRollingPivots(windowPeriods, offsetPeriods, pointType);
        }

        public static decimal GetResistanceLevel(IEnumerable<IQuote> quotes)
        {
            var pivots = GetRollingPivots(quotes, 20, 10);

            return pivots.LastOrDefault()?.R3 ?? 0m;
        }

        public static decimal GetSupportLevel(IEnumerable<IQuote> quotes)
        {
            var pivots = quotes.GetRollingPivots(20, 10);
            return (decimal)pivots.Last().S3;
        }

        public static decimal GetSma(IEnumerable<IQuote> quotes, int period)
        {
            var candles = ConvertToCandles(quotes);
            var sma = candles.GetSma(period).LastOrDefault();
            return (decimal)sma.Sma;
        }

        public static decimal GetStochasticRSI(IEnumerable<IQuote> quotes,
            int rsiPeriods, int stochPeriods, int signalPeriods, int smoothPeriods)
        {
            var candles = ConvertToCandles(quotes);
            var stochRsi = candles.GetStochRsi(rsiPeriods, stochPeriods, signalPeriods, smoothPeriods).LastOrDefault();
            return (decimal)stochRsi.StochRsi;
        }

        private static List<Candle> ConvertToCandles(IEnumerable<IQuote> quotes)
        {
            return quotes.Select(q => new Candle(q.Date, q.Open, q.High, q.Low, q.Close, q.Volume)).ToList();
        }

        public static decimal CalculateStochasticOverbought(List<QuoteDto> quotes, int rsiPeriods, int stochPeriods, int signalPeriods, int smoothPeriods)
        {
            var candles = quotes.Select(q => new Candle(q.Date, q.Open, q.High, q.Low, q.Close, q.Volume)).ToList();
            var stochRsi = candles.GetStochRsi(rsiPeriods, stochPeriods, signalPeriods, smoothPeriods).LastOrDefault();

            return (decimal)stochRsi.StochRsi;
        }

        public static async Task<decimal> CalculateSlopeAsync(IEnumerable<IQuote> quotes, int periods)
        {
            var results = await Task.Run(() => quotes.GetSlope(periods).ToList());
            var slopeResult = results.LastOrDefault();

            return (decimal)slopeResult.Slope;
        }

        public static decimal CalculateLinearRegression(IEnumerable<IQuote> quotes, int periods)
        {
            var closes = quotes.Select(q => q.Close).ToArray();
            var results = quotes.GetSlope(periods);
            var lastResult = results.LastOrDefault();

            if (lastResult == null) return 0m;

            var slope = (decimal)lastResult.Slope;
            var intercept = (decimal)lastResult.Intercept;
            var line = (decimal)lastResult.Line;

            return (slope * closes.Last()) + intercept + (line - closes.Last());
        }

        public static async Task<bool> IsUptrendAsync(List<QuoteDto> quotes, int smaPeriodShort, int smaPeriodLong)
        {
            var smaShortTask = Task.Run(() => Indicator.GetSma(quotes, smaPeriodShort).Last().Sma);
            var smaLongTask = Task.Run(() => Indicator.GetSma(quotes, smaPeriodLong).Last().Sma);

            await Task.WhenAll(smaShortTask, smaLongTask);

            return smaShortTask.Result > smaLongTask.Result;
        }

        public static async Task<bool> IsOversoldAsync(List<QuoteDto> quotes)
        {
            throw new NotImplementedException();
        }

        public static bool IsEngulfingCandle(IQuote previousQuote, IQuote lastQuote)
        {
            bool isBullishEngulfing = lastQuote.Close > previousQuote.Open && lastQuote.Open < previousQuote.Close;
            bool isBearishEngulfing = lastQuote.Close < previousQuote.Open && lastQuote.Open > previousQuote.Close;

            return isBullishEngulfing || isBearishEngulfing;
        }

        public static bool IsThreeLineStrike(List<QuoteDto> quotes)
        {
            if (quotes.Count < 4) return false;

            var lastQuote = quotes.Last();
            var prevQuotes = quotes.Skip(quotes.Count - 4).Take(3).ToList();

            bool isBullish(cond) => cond.Close < cond.Open;
            bool isBearish(cond) => cond.Close > cond.Open;

            bool isThreeLineStrikeBullish = prevQuotes.All(isBullish) &&
                                             lastQuote.Close > prevQuotes[0].Open &&
                                             lastQuote.Open < prevQuotes[2].Close;

            bool isThreeLineStrikeBearish = prevQuotes.All(isBearish) &&
                                             lastQuote.Close < prevQuotes[0].Open &&
                                             lastQuote.Open > prevQuotes[2].Close;

            return isThreeLineStrikeBullish || isThreeLineStrikeBearish;
        }

        public static bool IsRsiValidForPosition(decimal rsi, TradingSignal signal)
        {
            return signal == TradingSignal.GoLong ? rsi > 50 : rsi < 50;
        }

        public static bool IsFirstCandleCrossingSMA200(IEnumerable<QuoteDto> quotes)
        {
            var arr = quotes.ToArray();
            var sma200Results = Indicator.GetSma(quotes, 200).ToList();
            var lastQuote = quotes.Last();
            var prevQuote = arr[^2];

            return prevQuote.Close <= (decimal)sma200Results[^2].Sma &&
                   lastQuote.Close > (decimal)sma200Results.Last().Sma;
        }

        public static decimal CalculateStopLossPrice(IEnumerable<QuoteDto> quotes, bool isLong)
        {
            var lastQuote = quotes.Last();
            decimal candleLength = lastQuote.High - lastQuote.Low;
            decimal swingLow = quotes.OrderByDescending(q => q.Low).Skip(1).First().Low;

            return isLong
                ? Math.Min(lastQuote.Close - (2 * candleLength), swingLow)
                : Math.Max(lastQuote.Close + (2 * candleLength), swingLow);
        }

        public static bool CheckForBullishDivergence(List<IQuote> quotes, int rsiPeriods = 14)
        {
            IEnumerable<int> lowPoints = FindLowPoints(quotes);

            // Check if there is a higher low in the RSI
            foreach (var low in lowPoints)
            {
                var rsiValues = GetAllRsi(quotes.Skip(low).Take(rsiPeriods + 1), rsiPeriods);
                if (rsiValues.First() < rsiValues.Last())
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CheckForHiddenBullishDivergence(List<QuoteDto> quotes, decimal currentRsi, int rsiPeriods = 14)
        {
            // Check if there is a lower low in the price and a higher low in the RSI
            for (int i = quotes.Count - 2; i >= 0; i--)
            {
                var rsiValues = GetAllRsi(quotes.Skip(i).Take(rsiPeriods + 1), rsiPeriods);

                if (quotes[i].Low < quotes.Last().Low && (decimal)rsiValues.Last() > currentRsi)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CheckForBearishDivergence(List<IQuote> quotes, int rsiPeriods = 14)
        {
            IEnumerable<int> highPoints = FindHighPoints(quotes);

            // Check if there is a lower high in the RSI
            foreach (var high in highPoints)
            {
                var rsiValues = GetAllRsi(quotes.Skip(high).Take(rsiPeriods + 1), rsiPeriods);
                if (rsiValues.First() > rsiValues.Last())
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CheckForHiddenBearishDivergence(IEnumerable<QuoteDto> history, int oscillatorPeriod, int RsiPeriods)
        {
            var quotes = history.ToList();
            var oscillator = GetMomentumOscillator(quotes, oscillatorPeriod);

            // Find the peaks of the oscillator
            List<PeakPoint> peakPoints = GetPeakPoints(oscillator);

            if (peakPoints.Count < 2)
                return false;

            // Determine the most recent peak and the one before it
            var currentPeak = peakPoints.Last();
            var previousPeak = peakPoints[peakPoints.Count - 2];

            // Find the lows between the two peaks
            var lows = quotes.Skip(previousPeak.Index)
                             .Take(currentPeak.Index - previousPeak.Index + 1)
                             .Select(q => q.Low)
                             .ToList();

            List<IQuote> iquotes = (List<IQuote>)history.OfType<IQuote>();
            List<int> lowPoints = FindLowPoints(iquotes);

            // Determine if the RSI has formed a hidden bearish divergence pattern
            var previousRsiValue =
                GetAllRsi(quotes.GetRange(previousPeak.Index - RsiPeriods + 1, RsiPeriods + 1), RsiPeriods)
               .LastOrDefault();

            var currentRsi = quotes.GetPreviousRsi(RsiPeriods);
            var rsiIsDivergent = currentRsi < previousRsiValue;

            // Determine if the oscillator has formed a hidden bearish divergence pattern
            decimal previousOscillatorValue = oscillator[previousPeak.Index];
            var oscillatorIsDivergent = previousOscillatorValue < oscillator[currentPeak.Index];

            // Check if there is a bullish divergence between the RSI and the lows
            iquotes = (List<IQuote>)quotes.OfType<IQuote>();
            var rsiDivergence = CheckForBearishDivergence(iquotes, RsiPeriods);
            return rsiIsDivergent && oscillatorIsDivergent && rsiDivergence;
        }

        public static async Task<Tuple<bool, bool, bool, bool>> GetEmaSmmaCrossoverSignals(
            List<QuoteDto> quotes, QuoteDto lastQuote, int ema50Periods, int ema100Periods,
            int smma21Periods, int smma50Periods, int smma200Periods)
        {
            IEnumerable<EmaResult> ema20 = Indicator.GetEma(quotes, 20);
            IEnumerable<EmaResult> ema50 = Indicator.GetEma(quotes, ema50Periods);
            IEnumerable<EmaResult> ema100 = Indicator.GetEma(quotes, ema100Periods);

            IEnumerable<SmaResult> smma21 = Indicator.GetSma(quotes, smma21Periods);
            IEnumerable<SmaResult> smma50 = Indicator.GetSma(quotes, smma50Periods);
            IEnumerable<SmaResult> smma200 = Indicator.GetSma(quotes, smma200Periods);

            bool emaCrossedAbove = lastQuote.Close > (decimal)ema20.Last().Ema && lastQuote.Close > (decimal)ema50.Last().Ema;
            bool emaCrossedBelow = lastQuote.Close < (decimal)ema20.Last().Ema && lastQuote.Close < (decimal)ema50.Last().Ema;

            bool smmaCrossedAbove = lastQuote.Close > (decimal)smma21.Last().Sma && lastQuote.Close > (decimal)smma50.Last().Sma;
            bool smmaCrossedBelow = lastQuote.Close < (decimal)smma21.Last().Sma && lastQuote.Close < (decimal)smma50.Last().Sma;

            return Tuple.Create(emaCrossedAbove, emaCrossedBelow, smmaCrossedAbove, smmaCrossedBelow);
        }
    }
}