namespace TradeMonkey.DataCollector.Repositories
{
    [RegisterService]
    public sealed class TokenMetricsDbRepository
    {
        [InjectService]
        public TmDBContext DbContext { get; private set; }

        public TokenMetricsDbRepository(TmDBContext tmDBContext)
        {
            DbContext = tmDBContext ?? throw new ArgumentNullException(nameof(tmDBContext));
        }

        public async Task UpsertTokensAsync(IEnumerable<TokenMetrics_Token> tokens, CancellationToken ct = default)
        {
            await DbContext.TokenMetrics_Tokens.AddRangeAsync(tokens, ct);
            await DbContext.SaveChangesAsync(ct);
        }
    }
}