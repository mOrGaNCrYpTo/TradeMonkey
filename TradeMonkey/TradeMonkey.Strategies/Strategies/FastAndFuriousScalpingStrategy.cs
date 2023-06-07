using TradeMonkey.Trader.Classes;

namespace TradeMonkey.Trader.Strategies
{
    public sealed class FastAndFuriousScalpingStrategy : BaseChildStrategy
    {
        private TradingCalculators Calculators;

        public FastAndFuriousScalpingStrategy() : base(1) { }

        public override async Task<int> ExecuteStrategyAsync(TradeContext tradeContext)
        {
            // Let's use simple EMA crossover for scalping strategy Calculate EMA10 and EMA20
            var ema10 = Calculators.CalculateExponentialMovingAverage(tradeContext.Quotes, 10);
            var ema20 = Calculators.CalculateExponentialMovingAverage(tradeContext.Quotes, 20);

            // If EMA10 crosses above EMA20, it's a buy signal
            if (ema10 > ema20) return Weight;

            return 0;
        }
    }
}