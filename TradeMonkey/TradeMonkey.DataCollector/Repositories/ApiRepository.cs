namespace TradeMonkey.DataCollector.Repositories
{
    [RegisterService]
    public sealed class KucoinRepository
    {
        [InjectService]
        public KucoinClient KucoinClient { get; private set; }

        public KucoinRepository(KucoinClient kucoinClient)
        {
            KucoinClient = kucoinClient ?? throw new ArgumentNullException(nameof(kucoinClient));
        }

        // Get accounts and balances
        public async Task<WebCallResult<IEnumerable<KucoinAccount>>> GetAccountsAsync(CancellationToken token)
        {
            return await KucoinClient.SpotApi.Account.GetAccountsAsync(null, null, token);
        }

        // Getting the order book of a symbol
        public async Task<WebCallResult<IEnumerable<KucoinAsset>>> GetOrderBookAsync(CancellationToken token)
        {
            return await KucoinClient.SpotApi.ExchangeData.GetAssetsAsync(token);
        }

        // Getting info on all symbols
        public async Task<WebCallResult<IEnumerable<KucoinSymbol>>> GetSymbolsAsync(CancellationToken token)
        {
            return await KucoinClient.SpotApi.ExchangeData.GetSymbolsAsync(null, token);
        }

        // Getting assets
        public async Task<WebCallResult<KucoinTicks>> GetTickersAsync(CancellationToken token)
        {
            return await KucoinClient.SpotApi.ExchangeData.GetTickersAsync(token);
        }
    }
}