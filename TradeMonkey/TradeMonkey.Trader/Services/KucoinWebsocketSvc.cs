namespace TradeMonkey.DataCollector.Services
{
    [RegisterService]
    public sealed class KucoinWebsocketSvc
    {
        private readonly KucoinSocketClient _socketClient;

        public KucoinWebsocketSvc(KucoinSocketClient kucoinSocketClient) =>
            _socketClient = kucoinSocketClient ?? throw new ArgumentNullException(nameof(kucoinSocketClient));

        public async Task SubscribeToAllTickerUpdatesAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            // _socketClient.SubscribeToTickerUpdatesAsync()
        }
    }
}