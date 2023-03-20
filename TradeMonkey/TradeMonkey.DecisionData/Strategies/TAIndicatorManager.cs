namespace TradeMonkey.Trader.Strategies
{
    [RegisterService]
    public static class TAIndicatorManager
    {
        public static List<decimal> GetMomentumOscillator(IEnumerable<QuoteDto> quotes, int period)
        {
            // Calculate the momentum for the given period
            var momentum = new List<decimal>();
            for (int i = 0; i < quotes.Count(); i++)
            {
                if (i < period)
                {
                    momentum.Add(0);
                }
                else
                {
                    decimal currentPrice = quotes.ElementAt(i).Close;
                    decimal previousPrice = quotes.ElementAt(i - period).Close;
                    decimal change = currentPrice - previousPrice;
                    momentum.Add(change);
                }
            }

            // Calculate the simple moving average of the momentum
            var sma = new List<decimal>();
            for (int i = 0; i < momentum.Count(); i++)
            {
                if (i < period - 1)
                {
                    sma.Add(0);
                }
                else
                {
                    var sum = momentum.Skip(i - period + 1).Take(period).Sum();
                    var average = sum / period;
                    sma.Add(average);
                }
            }

            // Calculate the momentum oscillator by subtracting the current momentum from the simple
            // moving average
            var oscillator = new List<decimal>();
            for (int i = 0; i < momentum.Count(); i++)
            {
                if (i < (period * 2) - 2)
                {
                    oscillator.Add(0);
                }
                else
                {
                    var currentMomentum = momentum.ElementAt(i);
                    var currentSma = sma.ElementAt(i);
                    var value = currentMomentum - currentSma;
                    oscillator.Add(value);
                }
            }

            return oscillator;
        }

        public static List<PeakPoint> GetPeakPoints(this IEnumerable<decimal> values)
        {
            var peaks = new List<PeakPoint>();

            // Start with second value and end with second-to-last value
            for (int i = 1; i < values.Count() - 1; i++)
            {
                // Check if the value is a peak
                if (values.ElementAt(i) > values.ElementAt(i - 1) && values.ElementAt(i) > values.ElementAt(i + 1))
                {
                    peaks.Add(new PeakPoint { Value = values.ElementAt(i), Index = i });
                }
            }

            return peaks;
        }

        public static decimal GetCurrentRsi(IEnumerable<IQuote> quotes, int periods)
        {
            var rsi = quotes.GetRsi(periods)
                            .LastOrDefault()?.Rsi ?? default;

            return (decimal)rsi;
        }

        public static IEnumerable<decimal> GetAllRsi(IEnumerable<IQuote> quotes, int periods)
        {
            return quotes.GetRsi(periods)
                         .Select(rsiResult => (decimal)rsiResult.Rsi);
        }

        public static List<int> FindLowPoints(List<IQuote> quotes)
        {
            List<int> lowPoints = new List<int>();

            for (int i = 1; i < quotes.Count - 1; i++)
            {
                if (quotes[i].Low < quotes[i - 1].Low && quotes[i].Low < quotes[i + 1].Low)
                {
                    lowPoints.Add(i);
                }
            }

            return lowPoints;
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

            int lookbackPeriods = period + 100;

            var tr = Indicator.GetTr(candles);
            var atr = Indicator.GetAtr(candles, period);

            return (decimal)atr.LastOrDefault().Atr;
        }

        public static IEnumerable<RollingPivotsResult> GetRollingPivots(IEnumerable<IQuote> quotes, int windowPeriods, int offsetPeriods, PivotPointType pointType = PivotPointType.Standard)
        {
            var candles = quotes.Select(q => new Candle(q.Date, q.Open, q.High, q.Low, q.Close, q.Volume)).ToList();

            // Calculate pivot points using Skender.Stock.Indicators
            var pivots = candles.GetRollingPivots(windowPeriods, offsetPeriods, pointType);

            return pivots;
        }

        public static decimal GetResistanceLevel(IEnumerable<IQuote> quotes)
        {
            IEnumerable<RollingPivotsResult>? pivots = quotes?.GetRollingPivots(20, 10);

            return pivots.Any() ? (decimal)pivots.Last().R3 : 0m;
        }

        public static decimal GetSupportLevel(IEnumerable<IQuote> quotes)
        {
            var pivots = quotes.GetRollingPivots(20, 10);
            return (decimal)pivots.Last().S3;
        }

        public static decimal GetSma(IEnumerable<IQuote> quotes, int period)
        {
            // Convert quotes to Candle format
            List<Candle> candles = quotes
                .Select(q => new Candle(q.Date, q.Open, q.High, q.Low, q.Close, q.Volume))
                .ToList();

            // Calculate SMA using Skender.Stock.Indicators
            SmaResult sma = candles.GetSma(period).LastOrDefault();

            return (decimal)sma.Sma;
        }

        public static decimal GetStochasticRSI(IEnumerable<IQuote> quotes,
            int rsiPeriods, int stochPeriods, int signalPeriods, int smoothPeriods)
        {
            // Convert quotes to Candle format
            List<Candle> candles = quotes
                .Select(q => new Candle(q.Date, q.Open, q.High, q.Low, q.Close, q.Volume))
                .ToList();

            // Calculate Stochastic RSI using Skender.Stock.Indicators
            StochRsiResult stochRsi = candles
                                        .GetStochRsi(rsiPeriods, stochPeriods, signalPeriods, smoothPeriods)
                                        .LastOrDefault();

            return (decimal)stochRsi.StochRsi;
        }

        public static async Task<decimal> GetSlopeAsync(IEnumerable<IQuote> quotes, int periods)
        {
            var results = await Task.Run(() => quotes.GetSlope(periods).ToList());
            var slopeResult = results.LastOrDefault();
            return (decimal)slopeResult.Slope;
        }

        public static decimal GetLinearRegression(IEnumerable<IQuote> quotes, int periods)
        {
            var closes = quotes.Select(q => q.Close).ToArray();
            var results = quotes.GetSlope(periods);
            var lastResult = results.LastOrDefault();
            if (lastResult != null)
            {
                var slope = (decimal)lastResult.Slope;
                var intercept = (decimal)lastResult.Intercept;
                var line = (decimal)lastResult.Line;
                return (slope * closes.Last()) + intercept + (line - closes.Last());
            }
            return 0m;
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

        public static bool IsUptrend(List<QuoteDto> quotes, int smaPeriodShort, int smaPeriodLong)
        {
            var smaShort = Indicator.GetSma(quotes, smaPeriodShort).Last().Sma;
            var smaLong = Indicator.GetSma(quotes, smaPeriodLong).Last().Sma;

            return smaShort > smaLong;
        }

        public static bool IsEngulfingCandle(IQuote prevQuote, IQuote lastQuote)
        {
            bool isBullishEngulfing = lastQuote.Close > prevQuote.Open && lastQuote.Open < prevQuote.Close;
            bool isBearishEngulfing = lastQuote.Close < prevQuote.Open && lastQuote.Open > prevQuote.Close;

            return isBullishEngulfing || isBearishEngulfing;
        }

        public static bool IsThreeLineStrike(List<QuoteDto> quotes)
        {
            if (quotes.Count < 4)
            {
                return false;
            }

            var lastQuote = quotes.Last();
            var prevQuotes = quotes.Skip(quotes.Count - 4).Take(3).ToList();

            bool isThreeLineStrikeBullish = prevQuotes[0].Close < prevQuotes[0].Open &&
                                             prevQuotes[1].Close < prevQuotes[1].Open &&
                                             prevQuotes[2].Close < prevQuotes[2].Open &&
                                             lastQuote.Close > prevQuotes[0].Open &&
                                             lastQuote.Open < prevQuotes[2].Close;

            bool isThreeLineStrikeBearish = prevQuotes[0].Close > prevQuotes[0].Open &&
                                             prevQuotes[1].Close > prevQuotes[1].Open &&
                                             prevQuotes[2].Close > prevQuotes[2].Open &&
                                             lastQuote.Close < prevQuotes[0].Open &&
                                             lastQuote.Open > prevQuotes[2].Close;

            return isThreeLineStrikeBullish || isThreeLineStrikeBearish;
        }

        public static bool IsRsiValidForPosition(decimal rsi, TradingSignal signal)
        {
            return (signal == TradingSignal.GoLong && rsi > 50) || (signal == TradingSignal.GoShort && rsi < 50);
        }

        public static bool IsFirstCandleCrossingSMA200(IEnumerable<QuoteDto> quotes)
        {
            var arr = quotes.ToArray();
            var sma200Results = Indicator.GetSma(quotes, 200).ToList();
            var lastQuote = quotes.Last();
            var prevQuote = arr[^2];

            return prevQuote.Close <= (decimal)sma200Results[^2].Sma
                && lastQuote.Close > (decimal)sma200Results.Last().Sma;
        }

        public static decimal CalculateStopLossPrice(IEnumerable<QuoteDto> quotes, bool isLong)
        {
            var lastQuote = quotes.Last();
            var candleLength = lastQuote.High - lastQuote.Low;
            var swingLow = quotes.OrderByDescending(q => q.Low).Skip(1).First().Low;

            if (isLong)
            {
                var stopLossPrice = Math.Min(lastQuote.Close - (2 * candleLength), swingLow);
                return stopLossPrice;
            }
            else
            {
                var stopLossPrice = Math.Max(lastQuote.Close + (2 * candleLength), swingLow);
                return stopLossPrice;
            }
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

        public static bool CheckForHiddenBearishDivergence(IEnumerable<QuoteDto> history, int oscillatorPeriod)
        {
            var quotes = history.ToList();
            var oscillator = TAIndicatorManager.GetMomentumOscillator(quotes, oscillatorPeriod);

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
    }
}