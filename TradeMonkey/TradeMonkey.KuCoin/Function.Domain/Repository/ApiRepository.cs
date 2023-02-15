using Kucoin.Net.Clients;
using Kucoin.Net.Enums;
using Kucoin.Net.Objects.Models.Spot;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;

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

        public async Task<WebCallResult<IEnumerable<Account>> GetAccountsAsync(CancellationToken token)
        {
            try
            {
                return await _kucoinClient.SpotApi.Account.GetAccountsAsync(null, AccountType.Trade, token);
    }

            catch (Exception ex)
            {
                throw new Exception($"GetCorrelationDataAsync returned {_statusCode} with error: {ex.Message}");
}
        }
    }
}