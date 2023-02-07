namespace TradeMonkey.Function.Domain.Services
{
    [RegisterService]
    public sealed class TokenMetricsSvc
    {
        public TokenMetricsRepository TokenMetricsRepository { get; private set; }

        public TokenMetricsSvc(TokenMetricsRepository tokenMetricsRepository)
        {
            TokenMetricsRepository = tokenMetricsRepository
                ?? throw new ArgumentNullException(nameof(tokenMetricsRepository));
        }

        public Task<List<Token>> ExecuteAsync(List<int> TokenIds, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return TokenMetricsRepository.GetTokenDataAsync(TokenIds, token);
        }
    }
}