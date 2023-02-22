namespace TradeMonkey.DataCollector.Strategies
{
    [RegisterService]
    public class BreakoutStrategy
    {
        // Define some parameters for the breakout strategy
        static readonly int periodLength = 20;

        static readonly decimal breakoutThreshold = 1.05m;

        // Keep track of the highest high and lowest low over the last n periods
        static List<decimal> highPrices = new List<decimal>();

        static List<decimal> lowPrices = new List<decimal>();
        public int FastMA { get; set; } = 50;
        public int SlowMa { get; set; } = 200;

        public static async Task ProcessDataAsync(KucoinStreamTick tick, CancellationToken ct = default)
        {
            // Add the current price to the high/low price lists, if it's not null
            if (tick.LastPrice.HasValue)
            {
                highPrices.Add(tick.LastPrice.Value);
                lowPrices.Add(tick.LastPrice.Value);

                // Trim the lists to keep only the last n periods
                if (highPrices.Count > periodLength) highPrices.RemoveAt(0);
                if (lowPrices.Count > periodLength) lowPrices.RemoveAt(0);

                // Determine the highest high and lowest low over the last n periods
                var highestHigh = highPrices.Max();
                var lowestLow = lowPrices.Min();

                // Check for a breakout opportunity
                if (tick.LastPrice > highestHigh * breakoutThreshold)
                {
                }
                else if (tick.LastPrice < lowestLow * (2 - breakoutThreshold))
                {
                    // Trigger a sell order ...
                }
            }
        }
    }
}