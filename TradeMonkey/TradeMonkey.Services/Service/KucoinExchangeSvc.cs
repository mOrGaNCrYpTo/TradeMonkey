using Kucoin.Net.Clients.SpotApi;

using TradeMonkey.Services.Interface;

namespace TradeMonkey.Services.Service
{
    public sealed class KucoinExchangeSvc : ITraderService
    {
        private readonly KucoinClientSpotApiExchangeData Client;

        public KucoinExchangeSvc(KucoinClientSpotApiExchangeData client)
        {
            Client = client ??
                throw new ArgumentNullException(nameof(client));
        }
    }
}