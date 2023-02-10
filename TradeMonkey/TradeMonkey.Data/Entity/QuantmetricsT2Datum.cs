namespace TradeMonkey.Data.Entity
{
    public class QuantmetricsT2Datum
    {
        public float AvgDailyReturn { get; set; }
        public float AvgDownMonth { get; set; }
        public float AvgMontlyReturn { get; set; }
        public float AvgUpMonth { get; set; }
        public float BestDay { get; set; }
        public float BestMonth { get; set; }
        public string Date { get; set; }
        public int EndPeriod { get; set; }
        public int Epoch { get; set; }
        public float MaxDailyReturn { get; set; }
        public float MaxMonthlyReturn { get; set; }
        public float MinDailyReturn { get; set; }
        public float MinMonthlyReturn { get; set; }
        public string Name { get; set; }
        public int StartPeriod { get; set; }
        public float StdDailyReturn { get; set; }
        public float StdMonthlyReturn { get; set; }
        public string Symbol { get; set; }
        public int TokenId { get; set; }
        public float WorstDay { get; set; }
        public float WorstMonth { get; set; }
    }
}