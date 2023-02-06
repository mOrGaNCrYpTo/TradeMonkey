namespace TradeMonkey.Function.Domain.Value.Response
{
    public sealed class QuantmetricsT2 : Base
    {
        public QuantmetricsT2Datum[] Data { get; set; }
    }

    public class QuantmetricsT2Datum
    {
        #region Public Properties

        public float AVG_DAILY_RETURN { get; set; }
        public float AVG_DOWN_MONTH { get; set; }
        public float AVG_MONTHLY_RETURN { get; set; }
        public float AVG_UP_MONTH { get; set; }
        public float BEST_DAY { get; set; }
        public float BEST_MONTH { get; set; }
        public string DATE { get; set; }
        public int END_PERIOD { get; set; }
        public int EPOCH { get; set; }
        public float MAX_DAILY_RETURN { get; set; }
        public float MAX_MONTHLY_RETURN { get; set; }
        public float MIN_DAILY_RETURN { get; set; }
        public float MIN_MONTHLY_RETURN { get; set; }
        public string NAME { get; set; }
        public int START_PERIOD { get; set; }
        public float STD_DAILY_RETURN { get; set; }
        public float STD_MONTHLY_RETURN { get; set; }
        public string SYMBOL { get; set; }
        public int TOKEN_ID { get; set; }
        public float WORST_DAY { get; set; }
        public float WORST_MONTH { get; set; }

        #endregion Public Properties
    }
}