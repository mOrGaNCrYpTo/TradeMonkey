using TradeMonkey.Trader.Utils;

using KucoinAllTick = TradeMonkey.Data.Entity.KucoinAllTick;

namespace TradeMonkey.Trader.Extensions
{
    public static class KucoinAllTickExtensions
    {
        public static decimal CalculateProfit(this KucoinAllTick ticker, Indicator[] indicators)
        {
            if (indicators.Any(i => i.CheckCondition(ticker)))
            {
                return 0; //ticker.BuyPrice - ticker.SellPrice;
            }

            return 0;
        }

        public static decimal CalculateMovingAverage(this KucoinAllTick ticker, int period, List<decimal> prices)
        {
            // Calculate moving average
            decimal sum = prices.Sum();
            decimal ma = sum / period;
            return ma;
        }

        public static decimal CalculateRelativeStrengthIndex(this KucoinAllTick ticker, int period, List<decimal> prices)
        {
            // Calculate RSI
            decimal avgGain = 0;
            decimal avgLoss = 0;
            for (int i = 1; i < period; i++)
            {
                decimal difference = prices[i] - prices[i - 1];
                if (difference > 0)
                {
                    avgGain += difference;
                }
                else
                {
                    avgLoss += Math.Abs(difference);
                }
            }
            avgGain = avgGain / period;
            avgLoss = avgLoss / period;

            decimal rsi = 100 - (100 / (1 + (avgGain / avgLoss)));
            return rsi;
        }

        public static decimal CalculateStochasticOscillator(this KucoinAllTick ticker, List<decimal> prices, int period)
        {
            // Calculate Stochastic Oscillator
            decimal maxPrice = prices.Max();
            decimal minPrice = prices.Min();

            for (int i = 0; i < period; i++)
            {
                if (prices[i] > maxPrice)
                {
                    maxPrice = prices[i];
                }
                if (prices[i] < minPrice)
                {
                    minPrice = prices[i];
                }
            }
            decimal stochastic = 100 * (((decimal)ticker.AveragePrice - minPrice) / (maxPrice - minPrice));
            return stochastic;
        }
    }
}