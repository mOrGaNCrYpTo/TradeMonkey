namespace TradeMonkey.Core.Value.Aggregate
{
    public sealed class TokenMetricsAggregate
    {
        public decimal DayReturnPercentage { get; set; }
        public decimal TaGradePercentage { get; set; }
        public decimal QuantGradePercentage { get; set; }
        public decimal TmTraderGradePercentage { get; set; }
        public decimal DailyReturnPercentage { get; set; }
    }
}