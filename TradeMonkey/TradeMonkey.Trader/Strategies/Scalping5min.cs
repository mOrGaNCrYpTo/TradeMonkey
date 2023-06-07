using CryptoExchange.Net.CommonObjects;

using Skender.Stock.Indicators;

using TradeMonkey.Core;
using TradeMonkey.Data.Entity;
using TradeMonkey.Services.Repositories;
using TradeMonkey.Services.Service;
using TradeMonkey.Trader.Utils;
using TradeMonkey.Trader.Value.Constant;

using KucoinAllTick = TradeMonkey.Data.Entity.KucoinAllTick;

namespace TradeMonkey.Trader.Strategies
{
    public sealed class Scalping5min : BaseStrategy
    {
        // create a profit calculator
        //Calculators profitCalculator;

        private readonly TmDBContext tmDBContext = new();

        //SupportResistanceLevels = new List<decimal> { 1.00, 0.90, 0.80, 0.70, 0.60, 0.50 };
        private readonly KuCoinDbRepository _kucoinDbRepository;

        //Indicators.Add(new StochasticOscillator(14));
        private readonly KucoinTickerSvc _kuCoinTickerSvc;

        TradingInterval Timeframe = TradingInterval.FiveMinutes;

        //Indicators.Add(new MovingAverage(8, 14));

        //Indicators.Add(new RelativeStrengthIndex(14));
        public Scalping5min(KucoinTickerSvc kuCoinTickerSvc)
        {
        }

        public async Task RunAsync(KucoinAllTick tick)
        {
            var topCoins = await tmDBContext.TraderGradesDatums
                .OrderByDescending(x => x.QuantGrade)
                .Take(10)
                .Select(x => x.Symbol)
                .ToListAsync();

            // Retrieve ticker data from Kucoin API
            var ticks = await kucoinApi.GetTicks("ETH-USDT", 5); // get 5-minute ticks for ETH-USDT

            // Retrieve Token Metrics data for ETH
            var token = await DbContext.TokenMetricsTokens.FirstOrDefaultAsync(t => t.Symbol == "ETH");
            var sentiments = await DbContext.SentimentsDatums.Where(s => s.Token_Id == token.Id).ToListAsync();
            var resistanceSupport = await DbContext.ResistanceSupportDatums.Where(r => r.Token_Id == token.Id).ToListAsync();

            // Calculate resistance and support levels
            var resistance = resistanceSupport.Max(r => r.Level);
            var support = resistanceSupport.Min(r => r.Level);

            // Determine sentiment trend
            var sentimentTrend = sentiments.OrderByDescending(s => s.Epoch).Take(5).Average(s => s.PolarityIndex) > 0 ? "Bullish" : "Bearish";

            // Determine trade direction
            var tradeDirection = ticks.Last().Close > resistance ? "Short" : ticks.Last().Close < support ? "Long" : "None";

            // Determine trade size
            var account = await DbContext.KucoinAccounts.FirstOrDefaultAsync(a => a.currency == "USDT" && a.type == "trade");
            var tradeSize = account.available * 0.1;

            // Place trade if conditions are met
            if (sentimentTrend == "Bullish" && tradeDirection == "Long")
            {
                var order = new KucoinOrder()
                {
                    Symbol = "ETH-USDT",
                    Type = OrderType.Limit,
                    Side = OrderSide.Buy,
                    Price = ticks.Last().Close,
                    Quantity = tradeSize
                };
                var response = await KucoinApi.PlaceOrder(order);
            }
            else if (sentimentTrend == "Bearish" && tradeDirection == "Short")
            {
                var order = new KucoinOrder()
                {
                    Symbol = "ETH-USDT",
                    Type = OrderType.Limit,
                    Side = OrderSide.Sell,
                    Price = ticks.Last().Close,
                    Quantity = tradeSize
                };
                var response = await KucoinApi.PlaceOrder(order);
            }
        }

        // execute the trade
        private async Task ExecuteTrade(Ticker ticker)
        {
            // open trade
            Trade trade = OpenTrade(ticker);

            // monitor trade
            while (trade.Status == TradeStatus.Open)
            {
                // check conditions
                if (strategy.CheckConditions(ticker))
                {
                    // calculate profit
                    decimal currentProfit = profitCalculator.CalculateProfit(ticker);

                    // if profit has reached the target, close the trade
                    if (currentProfit >= 0.1)
                    {
                        CloseTrade(trade);
                    }
                }
            }
        }
    }