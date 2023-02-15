namespace TradeMonkey.KuCoin.Domain.Value.Response
{
    public class ResistanceSupport
    {
        public ResistanceSupportDatum[] Data { get; set; }
    }

    public class ResistanceSupportDatum
    {
        public int Epoch { get; set; }
        public int Level { get; set; }
        public string TokenId { get; set; }
    }
}