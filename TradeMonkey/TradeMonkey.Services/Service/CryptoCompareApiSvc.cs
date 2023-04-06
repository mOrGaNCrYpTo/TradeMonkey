using CryptoCompare;

namespace TradeMonkey.Services.Service
{
    [RegisterService]
    public class CryptoCompareApiSvc
    {
        private readonly CryptoCompareClient _cryptoCompareClient;

        [InjectService]
        public KuCoinDbRepository Repo { get; private set; }

        public CryptoCompareApiSvc(CryptoCompareClient cryptoCompareClient, KuCoinDbRepository kuCoinDbRepository)
        {
            //Set the HttpClient property to the passed in HttpClient
            _cryptoCompareClient = cryptoCompareClient ?? throw new ArgumentNullException(nameof(cryptoCompareClient));
            Repo = kuCoinDbRepository ?? throw new ArgumentNullException(nameof(kuCoinDbRepository));
        }

        public async Task GetCurrentHourlyOhlcv(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var tradingPairs = await Repo.GetTradingPairs();

            Parallel.ForEach(tradingPairs, async tradingPair =>
            {
                List<HistoryResponse> cryptoDatas = new List<HistoryResponse>();
                var pair = tradingPair.Split('/');
                var fSym = pair[0];
                var tSym = pair[1];
                var response = await _cryptoCompareClient.History.HourlyAsync(fSym, tSym, null, null, null, true, null, null);
                if (response.IsSuccessfulResponse)
                {
                    var data = response.Data;
                    cryptoDatas.AddRange((IEnumerable<HistoryResponse>)data);
                    await Repo.InsertManyAsync(cryptoDatas, ct);
                }
            });
        }

        public async Task GetHistoricalHourlyOhlcv(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var tradingPairs = await Repo.GetTradingPairs();

            List<DateTimeOffset> dtos = new List<DateTimeOffset>()
                {
                    DateTimeOffset.FromUnixTimeSeconds(1609459200),
                    DateTimeOffset.FromUnixTimeSeconds(1611919200),
                    DateTimeOffset.FromUnixTimeSeconds(1619313600),
                    DateTimeOffset.FromUnixTimeSeconds(1626708000),
                    DateTimeOffset.FromUnixTimeSeconds(1634102400),
                    DateTimeOffset.FromUnixTimeSeconds(1641496800),
                    DateTimeOffset.FromUnixTimeSeconds(1648891200),
                    DateTimeOffset.FromUnixTimeSeconds(1656285600),
                    DateTimeOffset.FromUnixTimeSeconds(1663651200),
                    DateTimeOffset.FromUnixTimeSeconds(1663718400),
                    DateTimeOffset.FromUnixTimeSeconds(1666084800),
                    DateTimeOffset.FromUnixTimeSeconds(1668451200),
                    DateTimeOffset.FromUnixTimeSeconds(1670817600),
                    DateTimeOffset.FromUnixTimeSeconds(1673184000),
                    DateTimeOffset.FromUnixTimeSeconds(1675550400),
                    DateTimeOffset.FromUnixTimeSeconds(1677916800),
                    DateTimeOffset.FromUnixTimeSeconds(1680283200),
                    DateTimeOffset.FromUnixTimeSeconds(1682638800)
                };

            Parallel.ForEach(tradingPairs, async tradingPair =>
            {
                List<HistoryResponse> cryptoDatas = new List<HistoryResponse>();

                var pair = tradingPair.Split('/');
                var fSym = pair[0];
                var tSym = pair[1];

                foreach (var dto in dtos)
                {
                    var response =
                        await _cryptoCompareClient.History.HourlyAsync(fSym, tSym, null, null, dto, true, null, null);

                    if (response.IsSuccessfulResponse)
                    {
                        var data = response.Data;

                        cryptoDatas.AddRange((IEnumerable<HistoryResponse>)data);

                        await Repo.InsertManyAsync(cryptoDatas, ct);
                    }
                }
            });
        }
    }
}