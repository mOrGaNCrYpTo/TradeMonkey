using TradeMonkey.Trader.Classes;

namespace TradeMonkey.Trader.Strategies
{
    public class MasterStrategy
    {
        private List<IStrategy<QuoteDto>> ChildStrategies { get; }

        public MasterStrategy()
        {
            ChildStrategies = new List<IStrategy<QuoteDto>>
            {
                new MacdStrategy(1),
                new RsiStrategy(1),
                // Add more strategies here
            };
        }

        public async Task<int> ExecuteStrategyAsync(TradeContext tradeContext)
        {
            int totalScore = 0;

            foreach (var strategy in ChildStrategies)
            {
                totalScore += await strategy.ExecuteStrategyAsync(tradeContext);
            }

            return totalScore;
        }

        public async Task<TradingSignal> GetTradingSignalAsync(IEnumerable<QuoteDto> history, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public sealed class QuoteDto : IQuote
        {
            public int Id { get; set; }

            public int TokenId { get; set; }

            public string Name { get; set; }

            public string Symbol { get; set; }

            public decimal CurrentPrice { get; set; }

            public decimal Open { get; set; }

            public decimal High { get; set; }

            public decimal Low { get; set; }

            public decimal Close { get; set; }

            public decimal Volume { get; set; }

            public int Epoch { get; set; }

            public DateTime Date { get; set; }
        }

        public async Task<(decimal, decimal)> GetStopLossTakeProfitPricesAsync(IEnumerable<QuoteDto> history, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
        }

        public async Task<decimal> GetExitPriceAsync(IEnumerable<QuoteDto> history, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
        }

        public async Task<decimal> GetPositionSizeAsync(IEnumerable<QuoteDto> history, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
        }

        public async Task<TimeSpan> GetDurationAsync(IEnumerable<QuoteDto> history, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
        }
    }
}