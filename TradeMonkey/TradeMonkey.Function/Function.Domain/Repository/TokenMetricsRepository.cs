namespace TradeMonkey.Function.Domain.Repository
{
    [RegisterService]
    public sealed class TokenMetricsRepository
    {
        private readonly HttpClient _httpClient;
        private HttpStatusCode _statusCode = HttpStatusCode.OK;

        public TokenMetriccRequest tokenMetriccRequest;
        public Uri url { get; set; }

        public TokenMetricsRepository(HttpClient httpClient) =>
            _httpClient = httpClient
                ?? throw new ArgumentNullException(nameof(httpClient));

        public async Task<List<Correlation>> GetCorrelationDataAsync(CancellationToken token)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                using var response = await _httpClient.SendAsync(request, token);
                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<Correlation>>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetCorrelationDataAsync returned {_statusCode} with error: {ex.Message}");
            }
        }

        // Get Indicator Data
        public async Task<List<Indicator>> GetIndicatorDataAsync(CancellationToken token)
        {
            try
            {
                var tokenIds = string.Join(",", TokenIds);
                var url = new Uri($"{_httpClient.BaseAddress}tokens?token_ids={tokenIds}");

                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await _httpClient.SendAsync(request, token);
                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<Indicator>>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetIndicatorDataAsync returned {_statusCode} with error: {ex.Message}");
            }
        }

        public async Task<List<Indicies>> GetIndiciesDataAsync(CancellationToken token)
        {
            try
            {
                var tokenIds = string.Join(",", TokenIds);
                var url = new Uri($"{_httpClient.BaseAddress}tokens?token_ids={tokenIds}");

                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await _httpClient.SendAsync(request, token);
                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<Indicies>>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetTokenDataAsync returned {_statusCode} with error: {ex.Message}");
            }
        }

        public async Task<List<InvestorGrades>> GetInvestorGradesAsync(CancellationToken token)
        {
            try
            {
                var tokenIds = string.Join(",", TokenIds);
                var url = new Uri($"{_httpClient.BaseAddress}tokens?token_ids={tokenIds}");

                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await _httpClient.SendAsync(request, token);
                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<InvestorGrades>>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetInvestorGrades returned {_statusCode} with error: {ex.Message}");
            }
        }

        public async Task<List<PricePrediction>> GetPricePredictionAsync(CancellationToken token)
        {
            try
            {
                var tokenIds = string.Join(",", TokenIds);
                var url = new Uri($"{_httpClient.BaseAddress}tokens?token_ids={tokenIds}");

                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await _httpClient.SendAsync(request, token);
                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<PricePrediction>>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetPricePredictionAsync returned {_statusCode} with error: {ex.Message}");
            }
        }

        public async Task<List<QuantmetricsT1>> GetQuantimetricsTier1Async(CancellationToken token)
        {
            try
            {
                var tokenIds = string.Join(",", TokenIds);
                var url = new Uri($"{_httpClient.BaseAddress}tokens?token_ids={tokenIds}");

                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await _httpClient.SendAsync(request, token);
                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<QuantmetricsT1>>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetQuantimetricsTier1Async returned {_statusCode} with error: {ex.Message}");
            }
        }

        public async Task<List<QuantmetricsT2>> GetQuantimetricsTier2Async(CancellationToken token)
        {
            try
            {
                var tokenIds = string.Join(",", TokenIds);
                var url = new Uri($"{_httpClient.BaseAddress}tokens?token_ids={tokenIds}");

                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await _httpClient.SendAsync(request, token);
                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<QuantmetricsT2>>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetQuantimetricsTier2Async returned {_statusCode} with error: {ex.Message}");
            }
        }

        public async Task<List<ResistanceSupport>> GetResistanceSupportAsync(CancellationToken token)
        {
            try
            {
                var tokenIds = string.Join(",", TokenIds);
                var url = new Uri($"{_httpClient.BaseAddress}tokens?token_ids={tokenIds}");

                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await _httpClient.SendAsync(request, token);
                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<ResistanceSupport>>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetResistanceSupportAsync returned {_statusCode} with error: {ex.Message}");
            }
        }

        public async Task<List<ScenarioAnalysis>> GetScenarioAnalysisAsync(CancellationToken token)
        {
            try
            {
                var tokenIds = string.Join(",", TokenIds);
                var url = new Uri($"{_httpClient.BaseAddress}tokens?token_ids={tokenIds}");

                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await _httpClient.SendAsync(request, token);
                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<ScenarioAnalysis>>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetScenarioAnalysisAsync returned {_statusCode} with error: {ex.Message}");
            }
        }

        // Get Sentiments
        public async Task<List<Sentiments>> GetSentimentsAsync(CancellationToken token)
        {
            try
            {
                var tokenIds = string.Join(",", TokenIds);
                var url = new Uri($"{_httpClient.BaseAddress}tokens?token_ids={tokenIds}");

                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await _httpClient.SendAsync(request, token);
                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<Sentiments>>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetSentimentsAsync returned {_statusCode} with error: {ex.Message}");
            }
        }

        public async Task<List<TraderGrades>> GetTraderGradesAsync(CancellationToken token)
        {
            try
            {
                var tokenIds = string.Join(",", TokenIds);
                var url = new Uri($"{_httpClient.BaseAddress}tokens?token_ids={tokenIds}");

                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await _httpClient.SendAsync(request, token);
                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<TraderGrades>>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetTraderGradesAsync returned {_statusCode} with error: {ex.Message}");
            }
        }

        // Get TradingIndicator
        public async Task<List<TradingIndicator>> GetTradingIndicatorAsync(CancellationToken token)
        {
            try
            {
                var tokenIds = string.Join(",", TokenIds);
                var url = new Uri($"{_httpClient.BaseAddress}tokens?token_ids={tokenIds}");

                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await _httpClient.SendAsync(request, token);
                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<TradingIndicator>>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetTradingIndicatorAsync returned {_statusCode} with error: {ex.Message}");
            }
        }
    }
}