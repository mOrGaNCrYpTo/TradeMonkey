using TradeMonkey.Trader.Services;

using KucoinAccount = Kucoin.Net.Objects.Models.Spot.KucoinAccount;

namespace TradeMonkey.Trader.Helpers
{
    public sealed class TradingCalculators
    {
        [InjectService]
        public KucoinAccountSvc KucoinAccountSvc { get; private set; }

        public TradingCalculators(KucoinAccountSvc accountSvc)
        {
            KucoinAccountSvc = accountSvc ?? throw new ArgumentNullException(nameof(accountSvc));
        }

        public async Task<decimal> GetQuantityToBuy(decimal entryPrice, decimal stopLossPrice,
            decimal riskPercent, string symbol, CancellationToken ct)
        {
            // Get the available balance of the trading account
            List<KucoinAccount> accounts =
                await KucoinAccountSvc.GetAccountsAsync(symbol, AccountType.Trade, ct);

            KucoinAccount? account = accounts.Any() ? accounts.First() : null;

            if (account == null)
            {
                throw new Exception("Kucoin Account is null");
            }

            var availableBalance = account?.Available ?? 0;

            // Calculate the maximum amount of money that can be risked
            var maxRiskAmount = availableBalance * riskPercent / 100;

            // Calculate the difference between the entry price and stop loss price
            var priceDiff = entryPrice - stopLossPrice;

            // Calculate the position size based on the maximum amount of money that can be risked
            // and the price difference
            var positionSize = maxRiskAmount / priceDiff;

            // Round the position size down to the nearest lot size
            var lotSize = SymbolLotSizes.GetLotSize(symbol);
            var quantity = Math.Floor(positionSize / lotSize) * lotSize;

            // Return the calculated quantity
            return quantity;
        }
    }
}