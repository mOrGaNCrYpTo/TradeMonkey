namespace TradeMonkey.Services.Repositories
{
    [RegisterService]
    public sealed class TokenMetricsApiRepository
    {
        private readonly HttpClient _httpClient;
        private HttpStatusCode statusCode = HttpStatusCode.OK;

        public Uri ActionUrl { get; set; } = null!;

        public TokenMetricsApiRepository(HttpClient httpClient) =>
            _httpClient = httpClient
                ?? throw new ArgumentNullException(nameof(httpClient));

        public async Task<string> GetAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            HttpStatusCode statusCode = HttpStatusCode.OK;

            try
            {
                Console.WriteLine(ActionUrl);
                using var request = new HttpRequestMessage(HttpMethod.Get, ActionUrl);
                using var response = await _httpClient.SendAsync(request, token);

                statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync() ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message} - URL: {ActionUrl}");
                throw new Exception($"GetEntityAsync returned {statusCode} with error: {ex.Message}");
            }
        }

        public async Task<string> PostAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, ActionUrl);
                using var response = await _httpClient.SendAsync(request, token);

                statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync() ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetEntityAsync returned {statusCode} with error: {ex.Message}");
            }
        }
    }
}