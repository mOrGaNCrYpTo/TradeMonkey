namespace TradeMonkey.Trader.Utils
{
    public static class Calculators
    {
        public static decimal CalculateCost(decimal price, decimal takerFeeRate)
        {
            var Cost
                = price * (1 + takerFeeRate);

            return Cost;
        }

        public static decimal CalculateRevenue(decimal price, decimal makerFeeRate)
        {
            var Revenue = price * (1 - makerFeeRate);

            return Revenue;
        }

        public static decimal CalculateProfit(decimal bestBidPrice, decimal bestAskPrice, Indicator[] indicators)
        {
            if (indicators.Any(i => i.CheckCondition(bestBidPrice, bestAskPrice)))
            {
                return bestBidPrice - bestAskPrice;
            }

            return 0;
        }

        public static decimal CalculateMovingAverage(List<decimal> prices, int period)
        {
            // Calculate moving average
            decimal sum = prices.Sum();

            decimal ma = sum / period;
            return ma;
        }

        public static decimal CalculateRelativeStrengthIndex(decimal[] prices, int period)
        {
            // Calculate RSI
            decimal avgGain = 0;
            decimal avgLoss = 0;
            for (int i = 1; i < period; i++)
            {
                decimal difference = prices[i] - prices[i - 1];
                if (difference > 0)
                {
                    avgGain += (decimal)difference;
                }
                else
                {
                    avgLoss += Math.Abs((decimal)difference);
                }
            }
            avgGain = avgGain / period;
            avgLoss = avgLoss / period;

            decimal rsi = 100 - (100 / (1 + (avgGain / avgLoss)));
            return rsi;
        }

        public static decimal CalculateStochasticOscillator(decimal[] prices, int period)
        {
            // Calculate Stochastic Oscillator
            decimal maxPrice = 0;
            decimal minPrice = decimal.MaxValue;
            for (int i = 0; i < period; i++)
            {
                if (prices[i] > maxPrice)
                {
                    maxPrice = (decimal)prices[i];
                }
                if (prices[i] < minPrice)
                {
                    minPrice = (decimal)prices[i];
                }
            }
            decimal stochastic = 100 * ((decimal)prices[period - 1] - minPrice) / (maxPrice - minPrice);
            return stochastic;
        }
    }
}