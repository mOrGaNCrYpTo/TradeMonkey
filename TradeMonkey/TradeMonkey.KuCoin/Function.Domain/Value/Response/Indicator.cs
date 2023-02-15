namespace TradeMonkey.KuCoin.Domain.Value.Response
{
    public class Indicator : Base
    {
        public IndicatorDatum[] Data { get; set; }
    }

    public class IndicatorDatum
    {
        public int Epoch { get; set; }
        public int LastTmGradeSignal { get; set; }
        public float TmGradePercHighCoins { get; set; }
        public int TmGradeSignal { get; set; }
        public float TotalCryptoMcap { get; set; }
    }
}