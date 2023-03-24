using TradeMonkey.Services.Interface;

namespace TradeMonkey.Trader.Services
{
    [RegisterService]
    public sealed class KucoinSocketSvc : ITraderService
    {
        [InjectService]
        public KucoinSocketClient SocketClient { get; private set; }

        [InjectService]
        public KuCoinDbRepository Repo { get; private set; }

        public KucoinSocketSvc(KucoinSocketClient client, KuCoinDbRepository repo)
        {
            SocketClient = client;
            Repo = repo;
        }

        public async Task SubscribeToAllTickerUpdatesAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var subscribeResult = await SocketClient.SpotStreams.SubscribeToAllTickerUpdatesAsync(data =>
            {
                // Handle ticker data
            });
        }

        public async Task SubscribeToOrderUpdatesAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var subscribeResult = await SocketClient.SpotStreams.SubscribeToOrderUpdatesAsync(data =>
            {
                // Handle order updates
            }, data =>
            {
                // Handle match updates
            }, ct);
        }

        public async Task SaveTickerStreamAggregateAsync(Kucoin.Net.Objects.Models.Spot.KucoinTick ticker,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            await Repo.InsertOneAsync(ticker, ct);
            Console.WriteLine("Saved");
        }
    }
}