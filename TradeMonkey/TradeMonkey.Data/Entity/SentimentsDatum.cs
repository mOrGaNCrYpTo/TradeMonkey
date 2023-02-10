namespace TradeMonkey.Data.Entity
{
    public class SentimentsDatum
    {
        public string Date { get; set; }
        public int Epoch { get; set; }
        public string Name { get; set; }
        public object PloarityReddit { get; set; }
        public float PolarityIndex { get; set; }
        public object PolarityTelegram { get; set; }
        public float PolarityTwitter { get; set; }
        public string SentimentIndex { get; set; }
        public string Symbol { get; set; }
        public int TokenId { get; set; }
    }
}