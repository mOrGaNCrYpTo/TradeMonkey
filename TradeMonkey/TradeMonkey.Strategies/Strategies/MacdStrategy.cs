using TradeMonkey.Trader.Rules;

namespace TradeMonkey.Trader.Strategies
{
    public class MacdStrategy : IStrategy<IQuote>
    {
        private readonly List<ITradingRule> _rules;
        private readonly ILogger _logger;

        public int Weight { get; private set; }
        public double Quantity { get; private set; }
        public TimeSpan Duration { get; private set; }
        public decimal ExitPrice { get; private set; }
        public (decimal, decimal) StopLossTakeProfit { get; private set; }

        public MacdStrategy(ILogger logger, int weight = 0)
        {
            _logger = logger;
            Weight = weight;

            _rules = new List<ITradingRule>
            {
                new RsiRule(period: 14, logger)
            };
        }

        public async Task<TradingSignal> ExecuteStrategyAsync(List<QuoteDto> quotes, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            foreach (var rule in _rules)
            {
                TradingSignal signal = await rule.GetTradingSignalAsync(quotes, ct);

                if (signal == TradingSignal.GoLong)
                { Weight += 1; }

                if (signal == TradingSignal.GoShort)
                { Weight -= 1; }
            }

            return Weight > 0 ? TradingSignal.GoLong : Weight < 0 ? TradingSignal.GoShort : TradingSignal.None;
        }
    }
}