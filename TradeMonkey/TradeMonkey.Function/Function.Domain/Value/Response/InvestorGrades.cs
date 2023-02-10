namespace TradeMonkey.TokenMetrics.Domain.Value.Response
{
    public class InvestorGrades : Base
    {
        public InvestorGradesDatum[] data { get; set; }
    }

    public class InvestorGradesDatum
    {
        public string Date { get; set; }
        public int Epoch { get; set; }
        public float FundamentalGrade { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public float TechnologyGrade { get; set; }
        public float TmInvestorGrade { get; set; }
        public int TokenId { get; set; }
        public float? ValuationGrade { get; set; }
    }
}