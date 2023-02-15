namespace TradeMonkey.KuCoin.Domain.Value.Response
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
        public string Date { get; set; }
        public int Epoch { get; set; }
        public string Name { get; set; }
        public float QuantGrade { get; set; }
        public string Symbol { get; set; }
        public float TaGrade { get; set; }
        public string TmTraderGrade { get; set; }
        public string TokenId { get; set; }
    }
}