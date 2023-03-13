namespace TradeMonkey.Services
{
    [RegisterService]
    public sealed class TokenMetricsSvc
    {
        private UriBuilder _uriBuilder;

        [InjectService]
        public TokenMetricsApiRepository ApiRepo { get; private set; }

        [InjectService]
        public TokenMetricsDbRepository DbRepo { get; private set; }

        public TokenMetricsSvc(TokenMetricsApiRepository apiRepository, TokenMetricsDbRepository dbRepository,
            UriBuilder uriBuilder)
        {
            ApiRepo = apiRepository ?? throw new ArgumentNullException(nameof(apiRepository));
            DbRepo = dbRepository ?? throw new ArgumentNullException(nameof(dbRepository));
            _uriBuilder = uriBuilder ?? throw new ArgumentNullException(nameof(uriBuilder));
        }

        public async Task GetAllTokens(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            _uriBuilder.Path = "Token";

            ApiRepo.ActionUrl = _uriBuilder.Uri;

            var json = await ApiRepo.GetAsync(ct);
            var tokenResponse = JsonSerializer.Deserialize<TokenMetricsTokenResponse>(json);

            if (tokenResponse != null && tokenResponse.Data.Any())
            {
                var tokens = tokenResponse.Data;
                await DbRepo.BulkInsertDataAsync(tokens, ct);
            }
        }

        public async Task<IEnumerable<TraderGradesDatum>?> GetTraderGradesAsync(List<int> symbols, string startDate,
            string endDate, int limit, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Console.WriteLine("GETTING TOKEN METRICS TRADER GRADES");

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNameCaseInsensitive = true
            };

            _uriBuilder.Path = "trader-grades";
            _uriBuilder.Query = $"tokens={string.Join(',', symbols)}&startDate={startDate}&endDate={endDate}&limit={limit}";

            ApiRepo.ActionUrl = _uriBuilder.Uri;

            var json = await ApiRepo.GetAsync(ct);
            var result = JsonSerializer.Deserialize<TokenMetricsTraderGradesResponse>(json, options);

            if (result != null && result.Data.Any())
            {
                await DbRepo.BulkInsertDataAsync(result.Data, ct);
                return result.Data;
            }

            return null;
        }

        public async Task<IEnumerable<TokenMetricsPrice>?> GetPricesAsync(List<int> symbols,
            string startDate, string endDate, int limit, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Console.WriteLine("GETTING TOKEN METRICS PRICES");
            Console.WriteLine("");

            _uriBuilder.Path = "Price";
            _uriBuilder.Query = $"tokens={string.Join(',', symbols)}&startDate={startDate}&endDate={endDate}&limit={limit}";

            ApiRepo.ActionUrl = _uriBuilder.Uri;

            var json = await ApiRepo.GetAsync(ct);
            var result = JsonSerializer.Deserialize<TokenMetricsPriceResponse>(json);

            if (result != null && result.Data.Any())
            {
                await DbRepo.BulkInsertDataAsync(result.Data, ct);
                return result.Data;
            }

            return null;
        }
    }
}