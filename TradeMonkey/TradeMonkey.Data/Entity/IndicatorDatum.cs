namespace TradeMonkey.Data.Entity
{
    public class IndicatorDatum
    {
        public int Epoch { get; set; }
        public int LastTmGradeSignal { get; set; }
        public float TmGradePercHighCoins { get; set; }
        public int TmGradeSignal { get; set; }
        public float TotalCryptoMcap { get; set; }
    }
}