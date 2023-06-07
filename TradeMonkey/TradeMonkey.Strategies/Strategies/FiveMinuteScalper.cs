using TradeMonkey.Trader.Classes;
using TradeMonkey.Trader.Interfaces;
using TradeMonkey.Trader.Rules;

namespace TradeMonkey.Trader
{
    public class FiveMinuteScalper
    {
        private readonly KucoinAccountSvc _kucoinAccountSvc;
        private readonly KucoinOrderSvc _orderService;
        private readonly TokenMetricsSvc _tokenMetricsSvc;
        private readonly TradingCalculators _tradingCalculators;
        private readonly Symbol _symbol;
        private readonly List<IStrategy<IQuote>> _strategies;
        private readonly ILogger _logger;

        public FiveMinuteScalper(
         KucoinAccountSvc kucoinAccountSvc,
         KucoinOrderSvc orderService,
         TokenMetricsSvc tokenMetricsSvc,
         Symbol symbol,
         ILogger<FiveMinuteScalper> logger)
        {
            _kucoinAccountSvc = kucoinAccountSvc ?? throw new ArgumentNullException(nameof(kucoinAccountSvc));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _tokenMetricsSvc = tokenMetricsSvc ?? throw new ArgumentNullException(nameof(tokenMetricsSvc));           
            _symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _strategies = new List<IStrategy<IQuote>>
            {
                new MacdStrategy(_logger, 1)
            };
        }

        public async Task ExecuteAsync(List<QuoteDto> quotes, decimal entryPrice, decimal risk, CancellationToken ct = default)
        {
            TradingSignal signal = TradingSignal.None;

            while (!ct.IsCancellationRequested)
            {
                foreach (IStrategy<IQuote> strategy in _strategies)
                {
                    signal = await strategy.ExecuteStrategyAsync(quotes, ct);

                    switch (signal)
                    {
                        case TradingSignal.GoLong:
                            var (stopLoss, takeProfit) =
                                await _tradingCalculators.GetStopLossTakeProfitPricesAsync(quotes);
                            var orderSide = OrderSide.Buy;
                            var quantity = await _tradingCalculators.GetQuantityToBuy(entryPrice, (decimal)stopLoss!, risk, _symbol.Name, ct);
                            var result = await _orderService.PostMarketOrderAsync(_symbol.Name, orderSide, quantity, ct);

                            if (result.Success)
                            {
                                _logger.LogInformation($"Buy order for {quantity} {_symbol.Name} placed at {DateTime.Now}");

                                decimal filledQuantity = quantity;

                                var stopLossOrder =
                                    _orderService.PostStopOrderAsync
                                    (
                                        symbol: _symbol.Name,
                                        orderSide: OrderSide.Sell,
                                        quantity: filledQuantity,
                                        tradeType: TradeType.SpotTrade,
                                        stopPrice: (decimal)stopLoss!,
                                        timeInForce: TimeInForce.GoodTillCanceled,
                                        cancelAfter: TimeSpan.FromDays(5),
                                        token: ct
                                    );

                                var takeProfitOrder =
                                    _orderService.PostStopOrderAsync
                                    (
                                        symbol: _symbol.Name,
                                        orderSide: OrderSide.Sell,
                                        quantity: filledQuantity,
                                        tradeType: TradeType.SpotTrade,
                                        stopPrice: (decimal)takeProfit!,
                                        timeInForce: TimeInForce.GoodTillCanceled,
                                        cancelAfter: TimeSpan.FromDays(5),
                                        token: ct
                                    );

                                List<Task> tasks = new List<Task> { stopLossOrder, takeProfitOrder };

                                await Task.WhenAll(tasks);
                            }
                            else
                            {
                                _logger.LogError($"Error placing buy order for {quantity} {_symbol.Name}");
                            }

                            break;

                        case TradingSignal.GoShort:
                            // Similar logic for opening a short position goes here
                            break;

                        case TradingSignal.None:
                        default:
                            break;
                    }
                }
            }
        }

        //private async Task<TradingSignal> GetTradingSignalAsync(List<QuoteDto> quotes)
        //{
        //    // Create instances of the trading rules
        //    var movingAveragesRule = new MovingAveragesRule { ShortPeriod = 50, LongPeriod = 100 };
        //    var macdRule = new MacdRule(smaFastPeriods: 12, smaSlowPeriods: 26, signalPeriods: 9);
        //    var rsiRule = new RsiRule { Period = 14 };
        //    var volumeRule = new VolumeRule();

        // // Add the rules to a list var tradingRules = new List<ITradingRule> {
        // movingAveragesRule, macdRule, rsiRule, volumeRule };

        // // Initialize points int goLongPoints = 0; int goShortPoints = 0;

        // // Evaluate each rule foreach (var rule in tradingRules) { var signal = await
        // rule.EvaluateAsync(quotes, CancellationToken.None);

        // switch (signal) { case TradingSignal.GoLong: goLongPoints++; break;

        // case TradingSignal.GoShort: goShortPoints++; break; } }

        //    // Determine the final signal based on the points
        //    if (goLongPoints > goShortPoints)
        //    {
        //        return TradingSignal.GoLong;
        //    }
        //    else if (goShortPoints > goLongPoints)
        //    {
        //        return TradingSignal.GoShort;
        //    }
        //    else
        //    {
        //        return TradingSignal.None;
        //    }
        //}

        public async Task ExecuteTradesAsync(TradeContext tradeContext, CancellationToken ct)
        {
            try
            {
                var stopLossOrderTask = PostStopOrderAsync(tradeContext.Symbol.Name, OrderSide.Sell, tradeContext.Quantity, tradeContext.StopLossPrice, ct);
                var takeProfitOrderTask = PostStopOrderAsync(tradeContext.Symbol.Name, OrderSide.Sell, tradeContext.Quantity, tradeContext.TakeProfitPrice, ct);

                await Task.WhenAll(stopLossOrderTask, takeProfitOrderTask);
            }
            catch (Exception ex)
            {
                Loggy.LogError($"Error placing stop or take profit order: {ex.Message}");
                throw;
            }
        }

        private async Task PostStopOrderAsync(string symbol, OrderSide orderSide, decimal quantity, decimal stopPrice, CancellationToken ct)
        {
            var result = await _orderService.PostStopOrderAsync
            (
                symbol: symbol,
                orderSide: orderSide,
                quantity: quantity,
                tradeType: TradeType.SpotTrade,
                stopPrice: stopPrice,
                timeInForce: TimeInForce.GoodTillCanceled,
                cancelAfter: TimeSpan.FromDays(5),
                token: ct
            );

            if (!result.Success)
            {
                Loggy.LogError($"Error placing stop order for {quantity} {symbol}");
                throw new Exception($"Error placing stop order for {quantity} {symbol}");
            }
        }
    }
}