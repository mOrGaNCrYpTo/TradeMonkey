using Kucoin.Net.Clients.SpotApi;

namespace TradeMonkey.Services.Service
{
    public sealed class KucoinExchangeSvc
    {
        private readonly KucoinClientSpotApiExchangeData Client;

        public KucoinExchangeSvc(KucoinClientSpotApiExchangeData client)
        {
            Client = client ??
                throw new ArgumentNullException(nameof(client));
        }
    }
}