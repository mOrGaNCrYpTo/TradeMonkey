using Newtonsoft.Json;

namespace TradeMonkey.Function.Domain.Repository
{
    [RegisterService]
    public sealed class TokenMetricsRepository
    {
        private readonly HttpClient _httpClient;

        public TokenMetricsRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Token>> GetTokenDataAsync(List<int> TokenIds, CancellationToken token)
        {
            try
            {
                var tokenIds = string.Join(",", TokenIds);
                var url = new Uri($"{_httpClient.BaseAddress}tokens?token_ids={tokenIds}");

                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await _httpClient.SendAsync(request, token);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<Token>>(content);

                return result;
            }
            catch(Exception ex)
            {
                throw new Exception($"TokenMetrics API returned {response.StatusCode}");
            }          
        }
    }
}
