using Mapster;

namespace TradeMonkey.DataCollector.Services
{
    [RegisterService]
    public sealed class KucoinTickerSvc
    {
        [InjectService]
        public KuCoinDbRepository Repo { get; private set; }

        [InjectService]
        public KucoinClient Client { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="repository"> </param>
        /// <exception cref="ArgumentNullException"> </exception>
        public KucoinTickerSvc(KuCoinDbRepository repository, KucoinClient kucoinClient)
        {
            Repo = repository ??
                throw new ArgumentNullException(nameof(repository));

            Client = kucoinClient ??
                throw new ArgumentNullException(nameof(kucoinClient));
        }

        public async Task GetLatestTickerDataAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var result = await Client.SpotApi.ExchangeData.GetTickersAsync(ct);
            var tickers = result.Data.Data.Adapt<IEnumerable<Kucoin_AllTick>>();

            await Repo.InsertTickerData(tickers, ct);
        }
    }
}