using TradeMonkey.Services;
using TradeMonkey.Services.Repositories;

namespace TradeMonkey.DataCollector.Strategies
{
    [RegisterService]
    public class BreakoutStrategy
    {
        [InjectService]
        public KucoinOrderSvc KucoinOrderSvc { get; private set; }

        [InjectService]
        public KuCoinDbRepository kuCoinDbRepository { get; private set; }

        [InjectService]
        public KucoinAccountSvc KucoinAccountSvc { get; private set; }

        public int FastMA { get; set; } = 50;
        public int SlowMa { get; set; } = 200;

        public BreakoutStrategy()
        {
        }

        //public async Task ProcessDataAsync(KucoinStreamTick tick, CancellationToken ct = default)
        //{
        //    ct.ThrowIfCancellationRequested();

        // var x = await KucoinAccountSvc.GetAccountsAsync(ct);

        // // assume that we have obtained the necessary data and stored it in a variable called "pricePrediction"

        // // set the threshold price based on your strategy double thresholdPrice = 100.0;

        // // get the current ticker data for the coin from Kucoin Kucoin_AllTick tickerData = await KucoinClient.Market.GetTickAsync("ETH-USDT");

        // // check if the predicted price exceeds the threshold price if (pricePrediction.High >
        // thresholdPrice) { // calculate the quantity of the coin to sell based on your strategy
        // decimal sellQuantity = tickerData.last / 2;

        // // place a sell order at the current market price var order = await
        // kucoinClient.Trade.PlaceOrderAsync(new KucoinPlaceOrderRequest { ClientOid =
        // Guid.NewGuid().ToString(), Side = OrderSide.Sell, Symbol = "ETH-USDT", Type =
        // OrderType.Limit, Price = tickerData.last, Quantity = sellQuantity, Remark = "Sell order
        // placed based on price prediction data", TimeInForce = TimeInForce.GTC }); }

        // // Add the current price to the high/low price lists, if it's not null if
        // (tick.LastPrice.HasValue) { highPrices.Add(tick.LastPrice.Value); lowPrices.Add(tick.LastPrice.Value);

        // // Get the historical prices for the period length IEnumerable<Kucoin_AllTick> tickers
        // = await DbRepository.GetTickerDataAsync(tick.Symbol, periodLength, ct);

        // highPrices.Add(tickers.Max(t => (decimal)t.AveragePrice)); lowPrices.Add(tickers.Min(t => (decimal)t.AveragePrice));

        // // Determine the highest high and lowest low over the last n periods var highestHigh =
        // highPrices.Max(); var lowestLow = lowPrices.Min();

        // // Method to calculate the quantity based on risk and available assets int quantity = 1;

        //        // Check for a breakout opportunity
        //        if (tick.LastPrice > highestHigh * breakoutThreshold)
        //        {
        //            await KucoinOrderSvc.PostMarketOrderAsync(tick.Symbol, OrderSide.Buy, quantity, ct);
        //        }
        //        else if (tick.LastPrice < lowestLow * (2 - breakoutThreshold))
        //        {
        //            await KucoinOrderSvc.PostMarketOrderAsync(tick.Symbol, OrderSide.Sell, quantity, ct);
        //        }
        //    }
        //}
    }
}