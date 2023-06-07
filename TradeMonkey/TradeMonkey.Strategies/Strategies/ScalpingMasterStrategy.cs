namespace TradeMonkey.Trader.Strategies
{
    public sealed class ScalpingMasterStrategy : MasterStrategy
    {
        public ScalpingMasterStrategy()
        {
            AddChildStrategy(new FastAndFuriousScalpingStrategy());
        }
    }
}