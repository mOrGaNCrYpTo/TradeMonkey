using KucoinAccount = Kucoin.Net.Objects.Models.Spot.KucoinAccount;

namespace TradeMonkey.Trader.Helpers
{
    [RegisterService]
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
                (List<KucoinAccount>)await KucoinAccountSvc.GetAccountsAsync(symbol, AccountType.Trade, ct);

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

        //public async Task<(decimal, decimal)> GetStopLossTakeProfitPricesAsync(IEnumerable<IQuote> history, CancellationToken ct = default)
        //{
        //    decimal entryPrice = history.Last().Close;
        //    decimal stopLossPrice = entryPrice * 0.99m; // stop loss set at 1% below the entry price
        //    decimal takeProfitPrice = entryPrice * 1.02m; // take profit set at 2% above the entry price
        //    return (stopLossPrice, takeProfitPrice);
        //}

        // TODO: Optimize
        public async Task<(decimal?, decimal?)> GetStopLossTakeProfitPricesAsync(IEnumerable<QuoteDto> history, CancellationToken ct = default)
        {
            var quotes = history.ToList();
            var price = quotes.Last().Close;
            var atr = TAIndicatorManager.GetAtr(quotes, 14);

            decimal stopLossMultiplier = 2; // Customize the multiplier based on your risk tolerance
            decimal takeProfitMultiplier = 3; // Customize the multiplier based on your desired reward ratio

            int rocPeriod = 9; // Customize the ROC period as needed
            decimal rocThreshold = 2.0M; // Customize the ROC threshold to determine fast movement

            var roc = TAIndicatorManager.GetRateOfChange(quotes, rocPeriod);

            // If the ROC value is above the threshold, indicating a fast upward movement, increase
            // the take profit multiplier
            if (roc > rocThreshold)
            {
                takeProfitMultiplier = 4; // Customize the increased multiplier based on your desired reward ratio
            }

            var stopLoss = price - (stopLossMultiplier * atr);
            var takeProfit = price + (takeProfitMultiplier * atr);

            return (stopLoss, takeProfit);
        }

        public async Task<TimeSpan> GetDurationAsync()
        {
            // Hold position for 1 hour, for example
            return TimeSpan.FromHours(1);
        }

        public async Task<decimal> GetExitPriceAsync(IEnumerable<IQuote> history, CancellationToken ct = default)
        {
            var (stopLossPrice, takeProfitPrice) = await GetStopLossTakeProfitPricesAsync(history, ct);
            decimal exitPrice = history.Last().Close;

            // Check if the price has hit the take profit or stop loss level
            if (exitPrice >= takeProfitPrice)
            {
                return takeProfitPrice;
            }
            else if (exitPrice <= stopLossPrice)
            {
                return stopLossPrice;
            }
            // If not, then exit at the current price
            else
            {
                return exitPrice;
            }
        }

        public decimal CalculateMovingAverage(List<QuoteDto> historicalPrices, int periods)
        {
            if (historicalPrices == null || !historicalPrices.Any())
            {
                throw new ArgumentException("Historical prices cannot be null or empty.", nameof(historicalPrices));
            }

            if (periods <= 0)
            {
                throw new ArgumentException("Periods must be greater than 0.", nameof(periods));
            }

            var recentPrices = historicalPrices.TakeLast(periods).ToList();

            return recentPrices.Average(x => x.Close);
        }

        public decimal CalculateExponentialMovingAverage(List<QuoteDto> historicalPrices, int periods)
        {
            if (historicalPrices == null || !historicalPrices.Any())
            {
                throw new ArgumentException("Historical prices cannot be null or empty.", nameof(historicalPrices));
            }

            if (periods <= 0)
            {
                throw new ArgumentException("Periods must be greater than 0.", nameof(periods));
            }

            // Calculate the weight
            var weight = 2.0m / (periods + 1);

            // First item is just the close price (could also be an SMA)
            var ema = historicalPrices[0].Close;

            // Apply formula for the rest of the data
            for (int i = 1; i < historicalPrices.Count; i++)
            {
                ema = (historicalPrices[i].Close - ema) * weight + ema;
            }

            return ema;
        }
    }
}