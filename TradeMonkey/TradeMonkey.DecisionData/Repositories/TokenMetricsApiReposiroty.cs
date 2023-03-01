namespace TradeMonkey.DataCollector.Repositories
{
    [RegisterService]
    public sealed class TokenMetricsApiReposiroty
    {
        private HttpStatusCode _statusCode;

        [InjectService]
        public TokenMetricsDbRepository Repo { get; private set; }

        public TokenMetricsApiReposiroty(TokenMetricsDbRepository tokenMetricsDbRepository)
        {
            Repo = tokenMetricsDbRepository ?? throw new ArgumentNullException(nameof(tokenMetricsDbRepository));
        }

        // Get accounts and balances
        public async Task GetTokensAsync(CancellationToken ct)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add(Settings.TokenMetricsApiKeyName, Settings.TokenMetricsApiKeyVal);

                using (var request = new HttpRequestMessage(HttpMethod.Get, Settings.TokenMetricsApiKeyVal))
                {
                    using (var response = httpClient.Send(request))
                    {
                        _statusCode = response.StatusCode;
                        response.EnsureSuccessStatusCode();

                        var content = await response.Content.ReadAsStringAsync(ct);
                        if (content != null)
                        {
                            JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            };

                            var result = JsonSerializer.Deserialize<TokenResponse>(content, jsonOptions);

                            try
                            {
                                if (result != null)
                                {
                                    await Repo.UpsertTokensAsync(result.Data, ct);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
        }
    }
}