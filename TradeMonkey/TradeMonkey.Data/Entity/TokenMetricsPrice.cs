namespace TradeMonkey.Data.Entity
{
    public partial class TokenMetricsPrice
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
}