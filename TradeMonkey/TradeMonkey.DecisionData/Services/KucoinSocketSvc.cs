namespace TradeMonkey.Trader.Services
{
    public sealed class KucoinSocketSvc
    {
        private readonly KucoinSocketClient _client;
        private readonly KuCoinDbRepository _repo;

        public KucoinSocketSvc(KucoinSocketClient client, KuCoinDbRepository repo)
        {
            _client = client;
            _repo = repo;
        }

        public async Task SubscribeToAllTickerUpdatesAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var subscribeResult = await _client.SpotStreams.SubscribeToAllTickerUpdatesAsync(data =>
            {
                // Handle ticker data
            });
        }

        public async Task SubscribeToOrderUpdatesAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var subscribeResult = await _client.SpotStreams.SubscribeToOrderUpdatesAsync(data =>
            {
                // Handle order updates
            },
            data =>
            {
                // Handle match updates
            });
        }
    }
}