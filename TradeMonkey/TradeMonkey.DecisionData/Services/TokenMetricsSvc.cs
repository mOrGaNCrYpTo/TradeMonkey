using TradeMonkey.Data.Entity;

namespace TradeMonkey.DecisionData.Services
{
    [RegisterService]
    public sealed class TokenMetricsSvc
    {
        [InjectService]
        public TokenMetricsApiRepository ApiRepo { get; private set; }

        [InjectService]
        public TokenMetricsDbRepository DbRepo { get; private set; }

        public TokenMetricsSvc(TokenMetricsApiRepository apiRepository, TokenMetricsDbRepository dbRepository)
        {
            ApiRepo = apiRepository ?? throw new ArgumentNullException(nameof(apiRepository));
            DbRepo = dbRepository ?? throw new ArgumentNullException(nameof(dbRepository));
        }

        public async Task GetAllTokens(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var builder = new UriBuilder(Settings.TokenMetricsApiBaseUrl)
            {
                Path = "v1/Tokens"
            };

            ApiRepo.ActionUrl = builder.Uri;

            var json = await ApiRepo.GetAsync(ct);
            var tokenResponse = JsonSerializer.Deserialize<TokenMetricsTokenResponse>(json);

            if (tokenResponse != null && tokenResponse.Data.Any())
            {
                var tokens = tokenResponse.Data;
                await DbRepo.UpsertDataAsync(tokens, ct);
            }
        }

        public async Task<IEnumerable<TraderGradesDatum>?> GetTraderGradesAsync(List<int> symbols, string startDate,
            string endDate, int limit, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var builder = new UriBuilder(Settings.TokenMetricsApiBaseUrl)
            {
                Path = "v1/trader-grades",
                Query = $"tokens={String.Join(',', symbols)}&startDate={startDate}&endDate={endDate}&limit={limit}"
            };

            ApiRepo.ActionUrl = builder.Uri;

            var json = await ApiRepo.GetAsync(ct);
            var result = JsonSerializer.Deserialize<TokenMetricsTraderGradesResponse>(json);

            if (result != null && result.Data.Any())
            {
                await DbRepo.UpsertDataAsync(result.Data, ct);
                return result.Data;
            }

            return null;
        }

        public async Task<IEnumerable<TradeMonkey.Data.Entity.TokenMetricsPrice>?> GetPricesAsync(List<int> symbols, string startDate,
            string endDate, int limit, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var builder = new UriBuilder(Settings.TokenMetricsApiBaseUrl)
            {
                Path = "v1/Price",
                Query = $"tokens={String.Join(',', symbols)}&startDate={startDate}&endDate={endDate}&limit={limit}"
            };

            ApiRepo.ActionUrl = builder.Uri;

            var json = await ApiRepo.GetAsync(ct);
            var result = JsonSerializer.Deserialize<TokenMetricsPriceResponse>(json);

            if (result != null && result.Data.Any())
            {
                await DbRepo.UpsertDataAsync(result.Data, ct);
                return result.Data;
            }

            return null;
        }
    }
}