namespace TradeMonkey.Function.Domain.Utillity
{
    internal static class Ta
    {
        public static decimal[] CalculateMovingAverage(decimal[] values, int period)
        {
            decimal[] maValues = new decimal[values.Length - period + 1];
            decimal sum = 0;

            for (int i = 0; i < period; i++)
            {
                sum += values[i];
            }

            maValues[0] = sum / period;

            for (int i = period; i < values.Length; i++)
            {
                sum += values[i] - values[i - period];
                maValues[i - period + 1] = sum / period;
            }

            return maValues;
        }
    }
}