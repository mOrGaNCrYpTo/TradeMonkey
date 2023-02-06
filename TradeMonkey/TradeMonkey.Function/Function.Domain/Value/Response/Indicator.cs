namespace TradeMonkey.Function.Domain.Value.Response
{
    public class Indicator : Base
    {
        public IndicatorDatum[] Data { get; set; }
    }

    public class IndicatorDatum
    {
        public int EPOCH { get; set; }
        public int LAST_TM_GRADE_SIGNAL { get; set; }
        public float TM_GRADE_PERC_HIGH_COINS { get; set; }
        public int TM_GRADE_SIGNAL { get; set; }
        public float TOTAL_CRYPTO_MCAP { get; set; }
    }
}