using Mapster;

using TradeMonkey.Data.Entity;
using TradeMonkey.DecisionData.Value.Aggregate;

namespace TradeMonkey.DataCollector.Strategies
{
    public sealed class Scalping5min
    {
        public Scalping5min()
        {
        }

        public async Task RunAsync(List<TokenMetricsPrice> prices)
        {
            var quotes = prices.Adapt<QuoteDto>();
        }
    }
}