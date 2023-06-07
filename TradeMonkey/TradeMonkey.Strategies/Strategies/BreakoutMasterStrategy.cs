namespace TradeMonkey.Trader.Strategies
{
    public sealed class BreakoutMasterStrategy : MasterStrategy
    {
        public BreakoutMasterStrategy()
        {
            AddChildStrategy(new CatchingFireBreakoutStrategy());
        }
    }
}