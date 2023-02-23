using Kucoin.Net.Clients.SpotApi;

namespace TradeMonkey.DataCollector.Services
{
    public sealed class KucoinWebsocketService
    {
        [InjectService]
        public TmDBContext DBContext { get; private set; }

        [InjectService]
        public KucoinSocketClientSpotStreams KucoinClient { get; private set; }

        [InjectService]
        public KucoinTickerDataSvc KucoinTickerDataSvc { get; private set; }

        public KucoinWebsocketService(TmDBContext dBContext, KucoinSocketClientSpotStreams kucoinClient, KucoinTickerDataSvc kucoinTickerDataSvc)
        {
            DBContext = dBContext ?? throw new ArgumentNullException(nameof(dBContext));
            KucoinClient = kucoinClient ?? throw new ArgumentNullException(nameof(kucoinClient));
            KucoinTickerDataSvc = kucoinTickerDataSvc ?? throw new ArgumentNullException(nameof(kucoinTickerDataSvc));
        }

        public async Task StartSubscription(CancellationToken ct)
        {
            await SubscribeToAllTickerUpdatesAsync(ct);
        }

        public async Task SubscribeToAllTickerUpdatesAsync(CancellationToken ct)
        {
            try
            {
                var result = await KucoinClient.SubscribeToAllTickerUpdatesAsync(async (DataEvent<KucoinStreamTick> dataEvent) =>
                {
                    // Handle the new ticker data here
                    KucoinStreamTick tick = dataEvent.Data;
                    Console.WriteLine($"Received new ticker data for {tick.Symbol}: last price = {tick.LastPrice}");

                    await KucoinTickerDataSvc.ProcessTick(tick, ct);
                });

                if (result.Success)
                {
                    Console.WriteLine("Subscribed to all ticker updates successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to subscribe to ticker updates: {result.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}