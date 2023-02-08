namespace TradeMonkey.TokenMetrics.Domain.Value.Response
{
    public class Sentiments : Base
    {
        public SentimentsDatum[] Data { get; set; }
    }

    public class SentimentsDatum
    {
        public string DATE { get; set; }
        public int EPOCH { get; set; }
        public string NAME { get; set; }
        public float POLARITY_INDEX { get; set; }
        public object POLARITY_REDDIT { get; set; }
        public object POLARITY_TELEGRAM { get; set; }
        public float POLARITY_TWITTER { get; set; }
        public string SENTIMENT_INDEX { get; set; }
        public string SYMBOL { get; set; }
        public int TOKEN_ID { get; set; }
    }
}