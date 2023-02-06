namespace TradeMonkey.Function.Domain.Value.Response
{
    /// <summary>
    /// Get the short term grades
    /// </summary>
    public class TraderGrades : Base
    {
        public TraderGradesDatum[] Data { get; set; }
    }

    public class TraderGradesDatum
    {
        public string DATE { get; set; }
        public int EPOCH { get; set; }
        public string NAME { get; set; }
        public float QUANT_GRADE { get; set; }
        public string SYMBOL { get; set; }
        public float TA_GRADE { get; set; }
        public string TM_TRADER_GRADE { get; set; }
        public string TOKEN_ID { get; set; }
    }
}