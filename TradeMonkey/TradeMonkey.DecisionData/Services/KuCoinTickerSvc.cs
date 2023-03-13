using Mapster;

using KucoinTick = TradeMonkey.Data.Entity.KucoinTick;

namespace TradeMonkey.Trader.Services
{
    [RegisterService]
    public sealed class KucoinTickerSvc
    {
        [InjectService]
        public KucoinClient _client { get; private set; }

        [InjectService]
        public KuCoinDbRepository Repo { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="repository"> </param>
        /// <exception cref="ArgumentNullException"> </exception>
        public KucoinTickerSvc(KuCoinDbRepository repository, KucoinClient kucoinClient)
        {
            Repo = repository ??
                throw new ArgumentNullException(nameof(repository));

            _client = kucoinClient ??
                throw new ArgumentNullException(nameof(kucoinClient));
        }

        public async Task GetAllTickerDataAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var result = await _client.SpotApi.ExchangeData.GetTickersAsync(ct);
            var data = result.Data.Data;
            var tickers = result.Data.Data.Adapt<IEnumerable<Kucoin.Net.Objects.Models.Spot.KucoinAllTick>>();

            await Repo.InsertManyAsync(tickers, ct);
        }

        public async Task GetLatestTickerDataAsync(string symbol, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Console.WriteLine("Getting latest Kucoin ticker data...");

            var result = await _client.SpotApi.ExchangeData.GetTickerAsync(symbol, ct);
            var data = result.Data;

            var ticker = new KucoinTick
            {
                BestAskPrice = data.BestAskPrice,
                BestBidPrice = data.BestBidPrice,
                LastPrice = data.LastPrice,
                Sequence = data.Sequence,
                Timestamp = data.Timestamp
            };

            //var tickers = data.Adapt<KucoinTick>();

            await Repo.InsertOneAsync(ticker, ct);
        }

        public async Task<List<string>> GetTopTokensAsync(int thresholdVolume, double thresholdChange, int numberOfTokens, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            var topTokens = await Repo.GetTopTokensAsync(thresholdVolume, thresholdChange, numberOfTokens, ct);

            List<string> symbols = new();
            symbols.AddRange(topTokens.HighVolumeDaily.Select(x => x.Symbol));
            symbols.AddRange(topTokens.SignificantChangeDaily.Select(x => x.Symbol));
            symbols.AddRange(topTokens.HighVolumeWeely.Select(x => x.Symbol));
            symbols.AddRange(topTokens.SignificantChangeWeekly.Select(x => x.Symbol));

            return symbols;
        }
    }
}