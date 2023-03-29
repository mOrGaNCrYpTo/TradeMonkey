using CoinAPI.REST.V1;

using Newtonsoft.Json;

namespace TradeMonkey.Services.Service
{
    [RegisterService]
    public class CoinApiService
    {
        private readonly CoinApiRestClient _coinApiRestClient;

        [InjectService]
        public KuCoinDbRepository Repo { get; private set; }

        public CoinApiService(CoinApiRestClient coinApiRestClient, KuCoinDbRepository kuCoinDbRepository)
        {
            //Set the HttpClient property to the passed in HttpClient
            _coinApiRestClient = coinApiRestClient ?? throw new ArgumentNullException(nameof(coinApiRestClient));
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
                var baseId = pair[0];
                var quoteId = pair[1];

                var response =
                    await _coinApiRestClient.Exchange_rates_get_specific_rateAsync(baseId, quoteId, DateTime.Parse(end));

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