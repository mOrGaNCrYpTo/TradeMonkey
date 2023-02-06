using System.Text;
using System.Text.Json;

namespace TradingViewHook
{
    public static class KuCoinAPI
    {
        // KuCoin API Base URL
        /*"https://api.kucoin.com/v1";*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="type">BUY or SELL</param>
        /// <param name="amount"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        /// https://docs.kucoin.com/#request
        private static async Task PlaceOrder(string type, string orderType, string symbol, double amount, double price,
            string baseUrl, string apiKey)
        {
            // Build the API endpoint
            string endpoint = $"{baseUrl}/order";

            var requestJson = new
            {
                type,
                orderType,
                symbol,
                amount,
                price
            };

            using var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

            var content = new StringContent(JsonSerializer.Serialize(requestJson));
            content.Headers.Add("KC-API-KEY", apiKey);
            content.Headers.Add("KC-API-SECRET", "YOUR_KUCOIN_API_SECRET");
            content.Headers.Add("KC-API-PASSPHRASE", "YOUR_KUCOIN_API_PASSPHRASE");
            content.Headers.Add("Content-Type", "application/json");

            var response = await client.PostAsync("/api/v1/orders", content);

            // Make the API call
            var ret = await client
                .PostAsync(endpoint, new StringContent(JsonSerializer.Serialize(requestJson), 
                           Encoding.UTF8, "application/json"));

            // Return the response as a string
            return await ret.Content.ReadAsStringAsync();

            //var valid = body != null ? await _restRepo.PostApi, SortedDictionary> (url, body, headers) : await _restRepo.PostApi > (url, headers);
        }
    }
}
