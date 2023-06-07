using Mapster;

using KucoinKline = TradeMonkey.Data.Entity.KucoinKline;
using KucoinTick = TradeMonkey.Data.Entity.KucoinTick;

namespace TradeMonkey.Services.Service
{
    [RegisterService]
    public sealed class KucoinTickerSvc
    {
        private readonly KucoinClient _kucoinClient;

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

            _kucoinClient = kucoinClient ??
                throw new ArgumentNullException(nameof(kucoinClient));
        }

        public async Task GetAllTickerDataAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var result = await _kucoinClient.SpotApi.ExchangeData.GetTickersAsync(ct);
            var data = result.Data.Data;
            var tickers = result.Data.Data.Adapt<IEnumerable<Kucoin.Net.Objects.Models.Spot.KucoinAllTick>>();

            await Repo.UpdateManyAsync(tickers, ct);
        }

        public async Task GetLatestTickerDataAsync(string symbol, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Console.WriteLine("Getting latest Kucoin ticker data...");

            var result = await _kucoinClient.SpotApi.ExchangeData.GetTickerAsync(symbol, ct);
            var data = result.Data;

            var ticker = new KucoinTick
            {
                BestAskPrice = data.BestAskPrice,
                BestBidPrice = data.BestBidPrice,
                LastPrice = data.LastPrice,
                Sequence = data.Sequence,
                Timestamp = data.Timestamp
            };

            await Repo.InsertOneAsync(ticker, ct);
        }

        public async Task GetTickerDataAsync(string symbol, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Console.WriteLine("Getting latest Kucoin ticker data...");

            var result = await _kucoinClient.SpotApi.ExchangeData.GetTickerAsync(symbol, ct);
            var data = result.Data;

            var ticker = new KucoinTick
            {
                BestAskPrice = data.BestAskPrice,
                BestBidPrice = data.BestBidPrice,
                LastPrice = data.LastPrice,
                Sequence = data.Sequence,
                Timestamp = data.Timestamp
            };

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

        //public async Task BackfillKucoinDataAsync(string symbols, DateTime start, DateTime end, CancellationToken ct)
        //{
        //    ct.ThrowIfCancellationRequested();
        //    List<string> assets = symbols.Split(',').ToList();
        //    List<KucoinKline> kucoinKlines = new List<KucoinKline>();
        //    //Kucoin.Net.Objects.Models.Spot.KucoinKline
        //    try
        //    {
        //        TimeSpan oneHour = new(0, 1, 0, 0, 0);

        // foreach (var asset in assets) { // Get historical OHLCV data from Kucoin for the
        // specified symbol var response = await _kucoinClient.SpotApi.CommonSpotClient
        // .GetKlinesAsync(asset, oneHour, start, end);

        // if (response.Success) { foreach (var k in response.Data) { // Create a new entity
        // KucoinKline kline = new() { OpenTime = k.OpenTime, OpenPrice = k.OpenPrice, ClosePrice =
        // k.ClosePrice, HighPrice = k.HighPrice, LowPrice = k.LowPrice, Volume = k.Volume };

        // // Add entity to the database context kucoinKlines.Add(kline); } } }

        //        await Repo.InsertManyAsync(kucoinKlines, ct);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Exception when calling Kucoin API: {e.Message}");
        //    }
        //}
    }
}