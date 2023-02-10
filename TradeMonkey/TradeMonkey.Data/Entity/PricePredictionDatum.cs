namespace TradeMonkey.Data.Entity
{
    public class PricePredictionDatum
    {
        public float Close { get; set; }
        public int Epoch { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public string Name { get; set; }
        public float Open { get; set; }
        public string Symbol { get; set; }
        public string TokenId { get; set; }
        public float Volume { get; set; }
    }
}