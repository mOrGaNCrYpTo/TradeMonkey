using Skender.Stock.Indicators;

using TradeMonkey.Trader.Value.Aggregate;

namespace TradeMonkey.Trader.Strategies
{
    [RegisterService]
    public class TAIndicatorManager : ITAIndicatorManager
    {
        public IEnumerable<decimal?> GetMomentumOscillator(IEnumerable<QuoteDto> quotes, int period)
        {
            // Calculate the momentum for the given period
            var momentum = new List<decimal?>();
            for (int i = 0; i < quotes.Count(); i++)
            {
                if (i < period)
                {
                    momentum.Add(null);
                }
                else
                {
                    var currentPrice = quotes.ElementAt(i).Close;
                    var previousPrice = quotes.ElementAt(i - period).Close;
                    var change = currentPrice - previousPrice;
                    momentum.Add(change);
                }
            }

            // Calculate the simple moving average of the momentum
            var sma = new List<decimal?>();
            for (int i = 0; i < momentum.Count(); i++)
            {
                if (i < period - 1)
                {
                    sma.Add(null);
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
            var oscillator = new List<decimal?>();
            for (int i = 0; i < momentum.Count(); i++)
            {
                if (i < (period * 2) - 2)
                {
                    oscillator.Add(null);
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

        public List<PeakPoint> GetPeakPoints(this IEnumerable<double> values)
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

        public decimal GetAtr(IEnumerable<IQuote> quotes, int period)
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

        public IEnumerable<RollingPivotsResult> GetRollingPivots(IEnumerable<IQuote> quotes, int windowPeriods, int offsetPeriods, PivotPointType pointType = PivotPointType.Standard)
        {
            var candles = quotes.Select(q => new Candle(q.Date, q.Open, q.High, q.Low, q.Close, q.Volume)).ToList();

            // Calculate pivot points using Skender.Stock.Indicators
            var pivots = candles.GetRollingPivots(windowPeriods, offsetPeriods, pointType);

            return pivots;
        }

        public decimal GetResistanceLevel(IEnumerable<IQuote> quotes)
        {
            IEnumerable<RollingPivotsResult>? pivots = quotes?.GetRollingPivots(20, 10);

            return pivots.Any() ? (decimal)pivots.Last().R3 : 0m;
        }

        public decimal GetSupportLevel(IEnumerable<IQuote> quotes)
        {
            var pivots = quotes.GetRollingPivots(20, 10);
            return (decimal)pivots.Last().S3;
        }

        public decimal GetSma(IEnumerable<IQuote> quotes, int period)
        {
            // Convert quotes to Candle format
            List<Candle> candles = quotes
                .Select(q => new Candle(q.Date, q.Open, q.High, q.Low, q.Close, q.Volume))
                .ToList();

            // Calculate SMA using Skender.Stock.Indicators
            SmaResult sma = candles.GetSma(period).LastOrDefault();

            return (decimal)sma.Sma;
        }

        public decimal GetRsi(IEnumerable<IQuote> quotes, int periods)
        {
            var closes = quotes.Select(q => q.Close).ToArray();
            var rsi = quotes.GetRsi(periods).LastOrDefault()?.Rsi ?? double.NaN;
            return (decimal)rsi;
        }

        public decimal GetStochasticRSI(IEnumerable<IQuote> quotes, int rsiPeriods, int stochPeriods, int signalPeriods, int smoothPeriods)
        {
            // Convert quotes to Candle format
            List<Candle> candles = quotes
                .Select(q => new Candle(q.Date, q.Open, q.High, q.Low, q.Close, q.Volume))
                .ToList();

            // Calculate Stochastic RSI using Skender.Stock.Indicators
            StochRsiResult stochRsi =
                candles.GetStochRsi(rsiPeriods, stochPeriods, signalPeriods, smoothPeriods).LastOrDefault();

            return (decimal)stochRsi.StochRsi;
        }

        public async Task<decimal> GetSlopeAsync(IEnumerable<IQuote> quotes, int periods)
        {
            var results = await Task.Run(() => quotes.GetSlope(periods).ToList());
            var slopeResult = results.LastOrDefault();
            return (decimal)slopeResult.Slope;
        }

        public decimal GetLinearRegression(IEnumerable<IQuote> quotes, int periods)
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

        public bool CheckForBullishDivergence(IEnumerable<IQuote> quotes, IEnumerable<int> lowPoints, int rsi)
        {
            // Check if there is a higher low in the RSI
            foreach (var low in lowPoints)
            {
                var rsiValues = IndicatorManager.GetRsi(quotes.Skip(low).Take(rsi + 1), rsi);
                if (rsiValues.First() < rsiValues.Last())
                {
                    return true;
                }
            }

            return false;
        }
    }
}