using CryptoExchange.Net.CommonObjects;

using TradeMonkey.DataCollector.Utils;

namespace TradeMonkey.DataCollector.Strategies
{
    public sealed class Scalping5min : BaseStrategy
    {
        //// create a profit calculator
        //Calculators profitCalculator = new Calculators();

        //Timeframe = TradingInterval.FiveMinutes;

        //Indicators.Add(new MovingAverage(8, 14));

        //Indicators.Add(new RelativeStrengthIndex(14));

        //Indicators.Add(new StochasticOscillator(14));

        //SupportResistanceLevels = new List<decimal> { 1.00, 0.90, 0.80, 0.70, 0.60, 0.50 };

        // execute the trade
        private async Task ExecuteTrade(Ticker ticker)
        {
            //// open trade
            //Trade trade = OpenTrade(ticker);

            //// monitor trade
            //while (trade.Status == TradeStatus.Open)
            //{
            //    // check conditions
            //    if (strategy.CheckConditions(ticker))
            //    {
            //        // calculate profit
            //        decimal currentProfit = profitCalculator.CalculateProfit(ticker);

            //        // if profit has reached the target, close the trade
            //        if (currentProfit >= 0.1)
            //        {
            //            CloseTrade(trade);
            //        }
            //    }
            //}
        }

        public async Task RunAsync(KucoinAllTick tick)
        {
            //// check if market conditions are met
            //if (strategy.CheckConditions(e.Ticker))
            //{
            //    // calculate profit potential
            //    decimal potentialProfit = profitCalculator.CalculateProfit(e.Ticker);

            //    // if potential profit is above threshold, execute trade
            //    if (potentialProfit > 0.1)
            //    {
            //        // execute trade
            //        await ExecuteTrade(e.Ticker);
            //    }
            //}
        }
    }
}