namespace TradeMonkey.KuCoin.Domain.Value.Response
{
    public class Sentiments : Base
    {
        public SentimentsDatum[] Data { get; set; }
    }

    public class SentimentsDatum
    {
        public string Date { get; set; }
        public int Epoch { get; set; }
        public string Name { get; set; }
        public object PloarityReddit { get; set; }
        public object POLARITY_TELEGRAM { get; set; }
        public float POLARITY_TWITTER { get; set; }
        public float PolarityIndex { get; set; }
        public string SENTIMENT_INDEX { get; set; }
        public string Symbol { get; set; }
        public int TokenId { get; set; }
    }
}