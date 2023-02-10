namespace TradeMonkey.Data.Entity
{
    public class CorrelationDatum
    {
        public float Correlation { get; set; }
        public string Date { get; set; }
        public int Epoch { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Token2Name { get; set; }
        public string Token2Symbol { get; set; }
        public int TokenId { get; set; }
    }
}