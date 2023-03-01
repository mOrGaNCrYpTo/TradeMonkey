namespace TradeMonkey.Data.Value
{
    public class TokenResponse
    {
        public List<TokenMetrics_Token> Data { get; set; }
        public int Length { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}