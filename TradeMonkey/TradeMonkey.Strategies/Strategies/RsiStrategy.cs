using TradeMonkey.Trader.Classes;

namespace TradeMonkey.Trader.Strategies
{
    public class RsiStrategy : BaseChildStrategy
    {
        public RsiStrategy(int weight) : base(weight)
        {
        }

        public override async Task<int> ExecuteStrategyAsync(TradeContext tradeContext)
        {
            // Get RSI value from tradeContext
            var rsi = tradeContext.Rsi;

            // Check for buy signal
            if (rsi < 30)
            {
                return Weight;
            }
            // Check for sell signal
            else if (rsi > 70)
            {
                return -Weight;
            }
            // No clear signal
            else
            {
                return 0;
            }
        }
    }