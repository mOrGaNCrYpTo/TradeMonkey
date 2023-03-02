namespace TradeMonkey.Data.Utils
{
    internal class SelfTradePreventionConverter : BaseConverter<SelfTradePrevention>
    {
        protected override List<KeyValuePair<SelfTradePrevention, string>> Mapping => new List<KeyValuePair<SelfTradePrevention, string>>
        {
            new KeyValuePair<SelfTradePrevention, string>(SelfTradePrevention.None, ""),
            new KeyValuePair<SelfTradePrevention, string>(SelfTradePrevention.DecreaseAndCancel, "DC"),
            new KeyValuePair<SelfTradePrevention, string>(SelfTradePrevention.CancelOldest, "CO"),
            new KeyValuePair<SelfTradePrevention, string>(SelfTradePrevention.CancelNewest, "CN"),
            new KeyValuePair<SelfTradePrevention, string>(SelfTradePrevention.CancelBoth, "CB"),
        };

        public SelfTradePreventionConverter() : this(true) { }

        public SelfTradePreventionConverter(bool quotes) : base(quotes) { }
    }
}