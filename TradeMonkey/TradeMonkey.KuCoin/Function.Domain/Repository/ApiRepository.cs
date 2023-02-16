using Kucoin.Net.Clients;
using Kucoin.Net.Enums;
using Kucoin.Net.Objects.Models.Spot;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using System.Net.Quic;

namespace TradeMonkey.KuCoin.Domain.Repository
{
    [RegisterService]
    public sealed class ApiRepository
    {
        private readonly KucoinClient _kucoinClient;
        private HttpStatusCode _statusCode = HttpStatusCode.OK;

        public Request Request { get; set; }
        public Uri Url { get; set; }

        public ApiRepository(KucoinClient kucoinClient)
        {
            _kucoinClient = kucoinClient
                ?? throw new ArgumentNullException(nameof(kucoinClient));
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
                    cancelAfter: null,
                    cancell token);
        }
    }
}