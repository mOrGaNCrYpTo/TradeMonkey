namespace TradeMonkey.Function.Domain.Value.Response
{
    public class ResistanceSupport
    {
        public ResistanceSupportDatum[] Data { get; set; }
    }

    public class ResistanceSupportDatum
    {
        public int EPOCH { get; set; }
        public int LEVEL { get; set; }
        public string TOKEN_ID { get; set; }
    }
}