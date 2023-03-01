using TradeMonkey.Data.Entity;
using TradeMonkey.DataCollector.Services;

namespace TradeMonkey.DataCollector.Strategies
{
    [RegisterService]
    public class BreakoutStrategy
    {
        // Define some parameters for the breakout strategy
        private readonly int periodLength = 20;

        private readonly decimal breakoutThreshold = 1.05m;

        // Keep track of the highest high and lowest low over the last n periods
        private readonly List<decimal> highPrices = new List<decimal>();

        private readonly List<decimal> lowPrices = new List<decimal>();

        private Kucoin.Net.Objects.Models.Spot.KucoinAccount account;

        public int FastMA { get; set; } = 50;
        public int SlowMa { get; set; } = 200;

        [InjectService]
        public KucoinOrderSvc KucoinOrderSvc { get; private set; }

        [InjectService]
        public DbRepository DbRepository { get; private set; }

        [InjectService]
        public KucoinAccountSvc KucoinAccountSvc { get; private set; }

        public BreakoutStrategy()
        {
        }

        public async Task ProcessDataAsync(KucoinStreamTick tick, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var x = await KucoinAccountSvc.GetAccountsAsync(ct);

            // Add the current price to the high/low price lists, if it's not null
            if (tick.LastPrice.HasValue)
            {
                highPrices.Add(tick.LastPrice.Value);
                lowPrices.Add(tick.LastPrice.Value);

                // Get the historical prices for the period length
                IEnumerable<Kucoin_AllTick> tickers
                  = await DbRepository.GetTickerDataAsync(tick.Symbol, periodLength, ct);

                highPrices.Add(tickers.Max(t => (decimal)t.averagePrice));
                lowPrices.Add(tickers.Min(t => (decimal)t.averagePrice));

                // Determine the highest high and lowest low over the last n periods
                var highestHigh = highPrices.Max();
                var lowestLow = lowPrices.Min();

                // Method to calculate the quantity based on risk and available assets
                int quantity = 1;

                // Check for a breakout opportunity
                if (tick.LastPrice > highestHigh * breakoutThreshold)
                {
                    await KucoinOrderSvc.PostMarketOrderAsync(tick.Symbol, OrderSide.Buy, quantity, ct);
                }
                else if (tick.LastPrice < lowestLow * (2 - breakoutThreshold))
                {
                    await KucoinOrderSvc.PostMarketOrderAsync(tick.Symbol, OrderSide.Sell, quantity, ct);
                }
            }
        }
    }
}