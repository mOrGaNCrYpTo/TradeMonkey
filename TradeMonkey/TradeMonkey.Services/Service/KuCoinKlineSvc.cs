using Mapster;

using KucoinKline = TradeMonkey.Data.Entity.KucoinKline;

namespace TradeMonkey.Services.Service
{
    public class KuCoinKlineSvc
    {
        private readonly KucoinClient _kucoinClient;

        [InjectService]
        public KuCoinDbRepository Repo { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="repository"> </param>
        /// <exception cref="ArgumentNullException"> </exception>
        public KuCoinKlineSvc(KuCoinDbRepository repository, KucoinClient kucoinClient)
        {
            Repo = repository ??
                throw new ArgumentNullException(nameof(repository));

            _kucoinClient = kucoinClient ??
                throw new ArgumentNullException(nameof(kucoinClient));
        }

        public async Task BackfillKucoinKlineData(List<string> assets, DateTime start, DateTime end, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            List<KucoinKline> kucoinKlines = new List<KucoinKline>();
            //Kucoin.Net.Objects.Models.Spot.KucoinKline
            try
            {
                TimeSpan oneHour = new(0, 1, 0, 0, 0);

                foreach (var asset in assets)
                {
                    // Get historical OHLCV data from Kucoin for the specified symbol
                    var response =
                        await _kucoinClient.SpotApi.CommonSpotClient
                            .GetKlinesAsync(asset, oneHour, start, end);

                    if (response.Success)
                    {
                        foreach (var k in response.Data)
                        {
                            // Create a new entity
                            KucoinKline kline = new()
                            {
                                OpenTime = k.OpenTime,
                                OpenPrice = k.OpenPrice,
                                ClosePrice = k.ClosePrice,
                                HighPrice = k.HighPrice,
                                LowPrice = k.LowPrice,
                                Volume = k.Volume
                            };

                            // Add entity to the database context
                            kucoinKlines.Add(kline);
                        }
                    }
                }

                await Repo.InsertManyAsync(kucoinKlines, ct);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception when calling Kucoin API: {e.Message}");
            }
        }

        public async Task GetKlinesAsync(string symbol,
            KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            Console.WriteLine("Getting latest Kucoin ticker data...");

            var result = await _kucoinClient.SpotApi.ExchangeData.GetKlinesAsync(symbol, interval,
                startTime, endTime, ct); var
            data = result.Data;

            var klines = data.Adapt<List<KucoinKline>>();

            await Repo.InsertManyAsync(klines, ct);

            return;
        }
    }
}