namespace TradeMonkey.Function.Domain.Value.Request
{
    public sealed class TokenMetricsRequest
    {
        public Dictionary<string, string> QueryStringKvps { get; set; } = new();
        public List<int> TokenIds { get; set; } = new();
        public string Url { get; set; } = string.Empty;

        //public DateOnly? Date { get; set; }
        //public DateOnly? EndDate { get; set; }
        //public int Limit { get; set; } = 500;
        //public DateOnly? StartDate { get; set; }
        //public string TimeHorizon { get; set; } = string.Empty;

        public string BuildUrl()
        {
            var sb = new StringBuilder($"{Url}?");

            if (TokenIds.Any())
            {
                sb.Append("tokens=");
                sb.AppendJoin(",", TokenIds);
                sb.Append('&');
            }

            foreach (var kvp in QueryStringKvps)
            {
                sb.Append($"{kvp.Key}={kvp.Value}&");
            }

            return sb.ToString();
        }
    }
}