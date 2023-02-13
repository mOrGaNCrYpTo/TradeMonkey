namespace TradeMonkey.Function.Domain.Utillity
{
    internal static class Ta
    {
        public static double[] CalculateMovingAverage(double[] values, int period)
        {
            double[] maValues = new double[values.Length - period + 1];
            double sum = 0;

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