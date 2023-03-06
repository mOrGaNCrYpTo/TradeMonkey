namespace TradeMonkey.DecisionData.Value.Aggregate
{
    public sealed class KucoinTopTokens
    {
        public List<TopTokenInfo> HighVolumeDaily { get; set; } = new();
        public List<TopTokenInfo> SignificantChangeDaily { get; set; } = new();
        public List<TopTokenInfo> HighVolumeWeely { get; set; } = new();
        public List<TopTokenInfo> SignificantChangeWeekly { get; set; } = new();
    }

    public sealed class TopTokenInfo
    {
        public string Symbol { get; set; } = string.Empty;
        public double Volume { get; set; }
        public double Change { get; set; }
    }
}