namespace TradeMonkey.TokenMetrics.Domain.Repository
{
    [RegisterService]
    public sealed class ApiRepository
    {
        private readonly HttpClient _httpClient;
        private HttpStatusCode _statusCode = HttpStatusCode.OK;

        public Request Request { get; set; }
        public Uri Url { get; set; }

        public ApiRepository(HttpClient httpClient)
        {
            _httpClient = httpClient
                ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<Correlation>> GetCorrelationDataAsync(CancellationToken token)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, Url);
                using var response = await _httpClient.SendAsync(request, token);

                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<Correlation>>(content);

                return result ?? new List<Correlation>();
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
                using var request = new HttpRequestMessage(HttpMethod.Get, Url);
                using var response = await _httpClient.SendAsync(request, token);

                _statusCode = response.StatusCode;

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<Indicator>>(content);

                return result ?? new List<Indicator>();
            }
            catch (Exception ex)
            {
                throw new Exception($"GetIndicatorDataAsync returned {_statusCode} with error: {ex.Message}");
            }
        }

        // Get Indicies Data
        public async Task<List<Indicies>> GetIndiciesDataAsync(CancellationToken token)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, Url);
                using var response = await _httpClient.SendAsync(request, token);

                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<Indicies>>(content);

                return result ?? new List<Indicies>();
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
                using var request = new HttpRequestMessage(HttpMethod.Get, Url);
                using var response = await _httpClient.SendAsync(request, token);

                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<InvestorGrades>>(content);

                return result ?? new List<InvestorGrades>();
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
                using var request = new HttpRequestMessage(HttpMethod.Get, Url);
                using var response = await _httpClient.SendAsync(request, token);

                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<PricePrediction>>(content);

                return result ?? new List<PricePrediction>();
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
                using var request = new HttpRequestMessage(HttpMethod.Get, Url);
                using var response = await _httpClient.SendAsync(request, token);

                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<QuantmetricsT1>>(content);

                return result ?? new List<QuantmetricsT1>();
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
                using var request = new HttpRequestMessage(HttpMethod.Get, Url);
                using var response = await _httpClient.SendAsync(request, token);

                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<QuantmetricsT2>>(content);

                return result ?? new List<QuantmetricsT2>();
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
                using var request = new HttpRequestMessage(HttpMethod.Get, Url);
                using var response = await _httpClient.SendAsync(request, token);

                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<ResistanceSupport>>(content);

                return result ?? new List<ResistanceSupport>();
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
                using var request = new HttpRequestMessage(HttpMethod.Get, Url);
                using var response = await _httpClient.SendAsync(request, token);

                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<ScenarioAnalysis>>(content);

                return result ?? new List<ScenarioAnalysis>();
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
                using var request = new HttpRequestMessage(HttpMethod.Get, Url);
                using var response = await _httpClient.SendAsync(request, token);

                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<Sentiments>>(content);

                return result ?? new List<Sentiments>();
            }
            catch (Exception ex)
            {
                throw new Exception($"GetSentimentsAsync returned {_statusCode} with error: {ex.Message}");
            }
        }

        public async Task<List<Token>> GetTokensAsync(CancellationToken token)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, Url);
                using var response = await _httpClient.SendAsync(request, token);

                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<Token>>(content);

                return result ?? new List<Token>();
            }
            catch (Exception ex)
            {
                throw new Exception($"GetCorrelationDataAsync returned {_statusCode} with error: {ex.Message}");
            }
        }

        public async Task<List<TraderGrades>> GetTraderGradesAsync(CancellationToken token)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, Url);
                using var response = await _httpClient.SendAsync(request, token);

                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<TraderGrades>>(content);

                return result ?? new List<TraderGrades>();
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
                using var request = new HttpRequestMessage(HttpMethod.Get, Url);
                using var response = await _httpClient.SendAsync(request, token);

                _statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<TradingIndicator>>(content);

                return result ?? new List<TradingIndicator>();
            }
            catch (Exception ex)
            {
                throw new Exception($"GetTradingIndicatorAsync returned {_statusCode} with error: {ex.Message}");
            }
        }
    }
}