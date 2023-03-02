using Kucoin.Net.Enums;

namespace TradeMonkey.Data.Utils
{
    internal class OrderSideConverter : BaseConverter<OrderSide>
    {
        protected override List<KeyValuePair<OrderSide, string>> Mapping => new List<KeyValuePair<OrderSide, string>>
        {
            new KeyValuePair<OrderSide, string>(OrderSide.Buy, "buy"),
            new KeyValuePair<OrderSide, string>(OrderSide.Sell, "sell")
        };

        public OrderSideConverter() : this(true) { }

        public OrderSideConverter(bool quotes) : base(quotes) { }
    }
}