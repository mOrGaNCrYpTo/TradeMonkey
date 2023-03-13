namespace TradeMonkey.Trader.Repositories
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
        public async Task<WebCallResult<IEnumerable<Kucoin.Net.Objects.Models.Spot.KucoinAccount>>> GetAccountsAsync(
            string? asset, AccountType? accountType, CancellationToken token)
        {
            return await GetAccountsAsync(asset, accountType, token);
        }

        // Getting the order book of a symbol
        public async Task<WebCallResult<IEnumerable<KucoinAsset>>> GetAggregatedFullOrderBookAsync(
            string symbol, CancellationToken token)
        {
            return await GetAggregatedFullOrderBookAsync(symbol, token);
        }

        // Getting info on all symbols
        public async Task<WebCallResult<IEnumerable<KucoinSymbol>>> GetSymbolsAsync(CancellationToken token)
        {
            return await GetSymbolsAsync(token);
        }

        // Getting assets
        public async Task<WebCallResult<KucoinTicks>> GetTickersAsync(CancellationToken token)
        {
            return await GetTickersAsync(token);
        }
    }
}