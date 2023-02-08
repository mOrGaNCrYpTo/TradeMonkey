namespace TradeMonkey.TokenMetrics.Domain.Value.Request
{
    public sealed class Request
    {
        public Dictionary<string, string> QueryStringKvps { get; set; } = new();
        public string TokenIds { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        public Uri BuildUrl()
        {
            var sb = new StringBuilder($"{Url}?");

            if (!string.IsNullOrEmpty(TokenIds))
            {
                sb.Append("tokens=");
                sb.Append(TokenIds);
            }

            foreach (var kvp in QueryStringKvps)
            {
                sb.Append($"{kvp.Key}={kvp.Value}&");
            }

            return new Uri(sb.ToString());
        }
    }
}