using Newtonsoft.Json;

namespace TradeMonkey.Services.Service
{
    [RegisterService]
    public class CoinApiService
    {
        private readonly HttpClient _httpClient;

        [InjectService]
        public KuCoinDbRepository Repo { get; private set; }

        public CoinApiService(HttpClient httpClient, KuCoinDbRepository kuCoinDbRepository)
        {
            //Set the HttpClient property to the passed in HttpClient
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            Repo = kuCoinDbRepository ?? throw new ArgumentNullException(nameof(kuCoinDbRepository));
        }

        public async Task GetCryptoData(string period, string start, string end, int limit = 100000,
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var tradingPairs = await Repo.GetTradingPairs();

            //Parallel.ForEach(tradingPairs, async tradingPair =>

            foreach (var tradingPair in tradingPairs)
            {
                List<CryptoData> cryptoDatas = new List<CryptoData>();

                var pair = tradingPair.Split('/');
                var asset = pair[0];
                var quote = pair[1];

                var url = $"{_httpClient.BaseAddress}/ohlcv/{asset}/{quote}/history?period_id={period}&time_start={start}&time_end={end}&limit={limit}";

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<CryptoData>>(json);

                    cryptoDatas.AddRange(data);

                    await Repo.InsertManyAsync(cryptoDatas, ct);
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve data for {asset}/{quote}: {response.ReasonPhrase}");
                }
                // });
            };
        }
    }
}