namespace TradeMonkey.TokenMetrics.Domain.Value.Response
{
    public class InvestorGrades : Base
    {
        public InvestorGradesDatum[] data { get; set; }
    }

    public class InvestorGradesDatum
    {
        public string DATE { get; set; }
        public int EPOCH { get; set; }
        public float FUNDAMENTAL_GRADE { get; set; }
        public string NAME { get; set; }
        public string SYMBOL { get; set; }
        public float TECHNOLOGY_GRADE { get; set; }
        public float TM_INVESTOR_GRADE { get; set; }
        public int TOKEN_ID { get; set; }
        public float? VALUATION_GRADE { get; set; }
    }
}