namespace TradeMonkey.Services
{
    [RegisterService]
    public sealed class KucoinOrderSvc
    {
        [InjectService]
        public KucoinClient KucoinClient { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="repository"> </param>
        /// <exception cref="ArgumentNullException"> </exception>
        public KucoinOrderSvc(KucoinClient kucoinClient)
        {
            KucoinClient = kucoinClient ??
                throw new ArgumentNullException(nameof(kucoinClient));
        }

        // Place a limit order
        public async Task<WebCallResult<KucoinNewOrder>> PostLimitOrderAsync(string symbol, OrderSide orderSide,
            int quantity, decimal limitPrice, CancellationToken token)
        {
            return await
                KucoinClient.SpotApi.Trading.PlaceOrderAsync(
                    symbol,
                    orderSide,
                    NewOrderType.Limit,
                    quantity: quantity,
                    price: limitPrice,
                    timeInForce: TimeInForce.GoodTillCanceled,
                    cancelAfter: null,
                    ct: token);
        }

        public async Task<WebCallResult<KucoinNewOrder>> PostMarketOrderAsync(string symbol, OrderSide orderSide,
           int quantity, CancellationToken token)
        {
            return await
                KucoinClient.SpotApi.Trading.
                    PlaceOrderAsync(symbol, orderSide,
                        NewOrderType.Market, quoteQuantity: quantity, ct: token);
        }

        public async Task<WebCallResult<KucoinNewOrder>> PostStopOrderAsync(string symbol, OrderSide orderSide,
           int quantity, TradeType tradeType, decimal stopPrice, TimeInForce timeInForce, TimeSpan cancelAfter,
           CancellationToken token)
        {
            return await
                KucoinClient.SpotApi.Trading.PlaceStopOrderAsync(
                    symbol,
                    orderSide,
                    NewOrderType.Limit,
                    StopCondition.Loss,
                    quantity: quantity,
                    tradeType: tradeType,
                    stopPrice: stopPrice,
                    timeInForce: timeInForce,
                    cancelAfter: cancelAfter,
                    ct: token);
        }

        public async Task CancelOrderAsync(string orderId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            await KucoinClient.SpotApi.Trading.CancelOrderAsync(orderId, ct);
        }
    }
}