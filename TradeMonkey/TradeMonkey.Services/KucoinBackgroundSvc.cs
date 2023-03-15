using Kucoin.Net.Objects.Models.Spot.Socket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TradeMonkey.Services
{
    namespace TradeMonkey.Trader.Services.Kucoin
    {
        public class KucoinBackgroundService : BackgroundService
        {
            private readonly ILogger<KucoinBackgroundService> _logger;
            private readonly IServiceProvider _serviceProvider;
            private readonly KucoinSocketClient _kucoinSocketClient;
            private List<KucoinStreamTick> _tickList;

            public KucoinBackgroundService(
                ILogger<KucoinBackgroundService> logger,
                IServiceProvider serviceProvider,
                KucoinSocketClient kucoinSocketClient)
            {
                _logger = logger;
                _serviceProvider = serviceProvider;
                _kucoinSocketClient = kucoinSocketClient;
                _tickList = new List<KucoinStreamTick>();
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                // Set up timer for aggregation process
                using var timer = new Timer(
                    OnKucoinStreamTimerElapsed,
                    state: "BTC-USDT",
                    dueTime: TimeSpan.Zero,
                    period: TimeSpan.FromMinutes(5)
                );

                // Subscribe to the KuCoin web-socket ticker stream
                await ReceivetickListAsync(_kucoinSocketClient, stoppingToken);

                // Wait for the cancellation token to be triggered
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }

            private void OnKucoinStreamTimerElapsed(object state)
            {
                string tradingPair = state as string;

                if (_tickList.Any())
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<TmDBContext>();

                    var aggregateTick = new Data.Entity.KucoinTick
                    {
                        Sequence = _tickList.Max(t => t.Sequence), // use the highest sequence number in the list
                        LastPrice = _tickList.Average(t => t.LastPrice), // compute the average last price
                        LastQuantity = _tickList.Sum(t => t.LastQuantity), // sum up the last quantities
                        BestAskPrice = _tickList.Min(t => t.BestAskPrice), // use the lowest ask price in the list
                        BestAskQuantity = _tickList.Sum(t => t.BestAskQuantity), // sum up the ask quantities
                        BestBidPrice = _tickList.Max(t => t.BestBidPrice), // use the highest bid price in the list
                        BestBidQuantity = _tickList.Sum(t => t.BestBidQuantity), // sum up the bid quantities
                        Timestamp = _tickList.Max(t => t.Timestamp), // use the latest timestamp in the list
                    };

                    _tickList.Clear();

                    dbContext.KucoinTicks.Add(aggregateTick);
                    dbContext.SaveChanges();
                }
            }

            private async Task ReceivetickListAsync(KucoinSocketClient client, CancellationToken ct)
            {
                // Subscribe to KuCoin websocket
                var subscribeResult = await client
                    .SpotStreams.SubscribeToTickerUpdatesAsync("ETH-BTC", async data =>
                    {
                        _tickList.Add(data.Data);
                    }, ct);

                //// Handle unsubscription and errors
                //await subscribeResult.Handle(async () =>
                //{
                //    // Unsubscribe when cancellationToken is triggered
                //    await client.SpotStreams.UnsubscribeFromTickerUpdatesAsync(subscribeResult.Data, ct);
                //}, ct);
            }
        }
    }
}