using Kucoin.Net.Clients.SpotApi;

namespace TradeMonkey.DataCollector.Strategies
{
    [RegisterService]
    public class BreakoutStrategy
    {
        // Define some parameters for the breakout strategy
        private static readonly int periodLength = 20;

        private static readonly decimal breakoutThreshold = 1.05m;

        private readonly TmDBContext _dbCtx;

        private readonly KucoinClientSpotApiTrading _kucoinClient;

        public BreakoutStrategy(TmDBContext dBContext, KucoinClientSpotApiTrading kucoinClient)
        {
            _dbCtx = dBContext;
            _kucoinClient = kucoinClient;
        }

        public async Task ProcessDataAsync(KucoinStreamTick streamtick, CancellationToken ct)
        {
            // Add the current price to the database
            if (streamtick.LastPrice.HasValue)
            {
                // Retrieve the last n periods of data from the database
                var prices = await _dbCtx.KucoinTicks
                    .Where(t => t.Symbol == streamtick.Symbol)
                    .OrderByDescending(p => p.Id)
                    .Take(periodLength)
                    .ToListAsync();

                // Extract the high and low prices from the retrieved data
                var highestHigh = prices.Select(p => p.BestAskPrice).Max();
                var lowestLow = prices.Select(p => p.BestAskPrice).Min();

                // Check for a breakout opportunity
                if (streamtick.LastPrice > highestHigh * breakoutThreshold)
                {
                    _ = await _kucoinClient.PlaceOrderAsync(streamtick.Symbol, OrderSide.Buy, NewOrderType.Market, quantity: 1);
                }
                else if (streamtick.LastPrice < lowestLow * (2 - breakoutThreshold))
                {
                    _ = await _kucoinClient.PlaceOrderAsync(streamtick.Symbol, OrderSide.Buy, NewOrderType.Market, quantity: 1);
                }
            }
        }
    }
}