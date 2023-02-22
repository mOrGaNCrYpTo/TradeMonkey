namespace TradeMonkey.DataCollector.Repositories
{
    [RegisterService]
    public sealed class ApiRepository
    {
        private readonly KucoinClient _kucoinClient;

        public ApiRepository(KucoinClient kucoinClient)
        {
            _kucoinClient = kucoinClient ?? throw new ArgumentNullException(nameof(kucoinClient));
        }

        // Get accounts and balances
        public async Task<WebCallResult<IEnumerable<KucoinAccount>>> GetAccountsAsync(CancellationToken token)
        {
            return await _kucoinClient.SpotApi.Account.GetAccountsAsync(null, null, token);
        }

        // Getting the order book of a symbol
        public async Task<WebCallResult<IEnumerable<KucoinAsset>>> GetOrderBookAsync(CancellationToken token)
        {
            return await _kucoinClient.SpotApi.ExchangeData.GetAssetsAsync(token);
        }

        // Getting info on all symbols
        public async Task<WebCallResult<IEnumerable<KucoinSymbol>>> GetSymbolsAsync(CancellationToken token)
        {
            return await _kucoinClient.SpotApi.ExchangeData.GetSymbolsAsync(null, token);
        }

        // Getting assets
        public async Task<WebCallResult<KucoinTicks>> GetTickersAsync(CancellationToken token)
        {
            return await _kucoinClient.SpotApi.ExchangeData.GetTickersAsync(token);
        }

        // Place a limit order
        public async Task<WebCallResult<KucoinNewOrder>> PostLimitOrder(string symbol,
            int quantity, decimal limitPrice, CancellationToken token)
        {
            return await
                _kucoinClient.SpotApi.Trading.PlaceOrderAsync(
                    symbol,
                    OrderSide.Buy,
                    NewOrderType.Limit,
                    quantity: quantity,
                    price: limitPrice,
                    timeInForce: TimeInForce.GoodTillCanceled,
                    cancelAfter: null);
        }

        public async Task<WebCallResult<KucoinNewOrder>> PostMarketOrderAsync(string symbol, OrderSide orderSide,
           int quantity, CancellationToken token)
        {
            return await
                _kucoinClient.SpotApi.Trading.PlaceOrderAsync(
                    symbol,
                    orderSide,
                    NewOrderType.Market,
                    quoteQuantity: quantity);
        }

        public async Task<WebCallResult<KucoinNewOrder>> PostStopOrderAsync(string symbol,
           int quantity, TradeType tradeType, decimal stopPrice, TimeInForce timeInForce, TimeSpan cancelAfter,
           CancellationToken token)
        {
            return await
                _kucoinClient.SpotApi.Trading.PlaceStopOrderAsync(
                    symbol,
                    OrderSide.Buy,
                    NewOrderType.Limit,
                    StopCondition.Loss,
                    quantity: quantity,
                    tradeType: tradeType,
                    stopPrice: stopPrice,
                    timeInForce: timeInForce,
                    cancelAfter: cancelAfter
                    );
        }
    }
}