using Kucoin.Net.Enums;

namespace TradeMonkey.Data.Utils
{
    internal class OrderTypeConverter : BaseConverter<OrderType>
    {
        protected override List<KeyValuePair<OrderType, string>> Mapping => new List<KeyValuePair<OrderType, string>>
        {
            new KeyValuePair<OrderType, string>(OrderType.Limit, "limit"),
            new KeyValuePair<OrderType, string>(OrderType.Market, "market"),
            new KeyValuePair<OrderType, string>(OrderType.LimitStop, "limit_stop"),
            new KeyValuePair<OrderType, string>(OrderType.MarketStop, "market_stop"),
            new KeyValuePair<OrderType, string>(OrderType.Stop, "stop"),
        };

        public OrderTypeConverter() : this(true) { }

        public OrderTypeConverter(bool quotes) : base(quotes) { }
    }
}