namespace TradeMonkey.Console.Helpers
{
    internal static class TimerHelpers
    {
        static SemaphoreSlim slimLock = new SemaphoreSlim(1, 1);
        static List<Kucoin.Net.Objects.Models.Spot.Socket.KucoinStreamTick> tickList = new();

        public static async Task ExecuteTokenMetricsTimers(ServiceProvider serviceProvider, CancellationToken ct)
        {
            var tokenMetricsSvc = serviceProvider.GetRequiredService<TokenMetricsSvc>();

            // ADOPHIS: We need to make these dynamic
            var batch = new List<int> { 3375, 3306, 3379 };
            var symbols = new List<string> { "ETH", "BTC" };

            // Now get the trader grades for the top tokens
            DateTime currentDateTime = DateTime.Now;
            var startDate = DateOnly.FromDateTime(currentDateTime.AddDays(-1));
            var endDate = DateOnly.FromDateTime(currentDateTime);
            var limit = 1000000;

            var tokenMetricsPrices = await tokenMetricsSvc.GetPricesAsync(batch, startDate, endDate, limit, ct);
            var tokenMetricsGrades = await tokenMetricsSvc.GetTraderGradesAsync(batch, startDate, endDate, limit, ct);
            //var tokenMetricsIndicator = await tokenMetricsSvc.GetIndicatorAsync(symbols, startDate, endDate, limit, ct);
            var tokenMetricsResistanceSupport = await tokenMetricsSvc.GetResistanceSupportAsync(symbols, startDate, endDate, limit, ct);
        }

        public static async Task ExecuteKuCoinTimers(ServiceProvider serviceProvider, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                var dbContext = serviceProvider.GetRequiredService<TmDBContext>();
                var kucoinSocketClient = serviceProvider.GetRequiredService<KucoinSocketClient>();
                var kucoinAccountSvc = serviceProvider.GetRequiredService<KucoinAccountSvc>();

                // Stream aggregation process every -Default 5 minutes (300000 ms)
                var state = new TimerState { TradingPair = "ETH-BTC" };
                var timer = new Timer((s) => Task.Run(() => OnKucoinStreamTimerElapsed(s, dbContext, ct)), state, 0, 300000);

                // Ticker Websocket subscription
                await ReceivetickListAsync(kucoinSocketClient);

                // Get the symbols of the coins you're holding

                var accountBalances = await kucoinAccountSvc.GetAccountsAsync(null, null, ct);
                var assets = accountBalances
                    .Where(s => s.Holds > 0)
                    .Select(s => s.Asset)
                    .ToList();

                // Define the time range for historical data
                var endTime = DateTime.UtcNow;
                var startTime = endTime.AddYears(-1);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }

        static async Task OnKucoinStreamTimerElapsed(object state, TmDBContext dbContext, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            System.Console.WriteLine("Elapsed..");

            TimerState timerState = (TimerState)state;
            string tradingPair = timerState.TradingPair;

            await slimLock.WaitAsync();
            try
            {
                if (tickList.Any())
                {
                    var aggregateTick = new TradeMonkey.Data.Entity.KucoinTick
                    {
                        Sequence = tickList.Max(t => t.Sequence), // use the highest sequence number in the list
                        LastPrice = tickList.Average(t => t.LastPrice), // compute the average last price
                        LastQuantity = tickList.Sum(t => t.LastQuantity), // sum up the last quantities
                        BestAskPrice = tickList.Min(t => t.BestAskPrice), // use the lowest ask price in the list
                        BestAskQuantity = tickList.Sum(t => t.BestAskQuantity), // sum up the ask quantities
                        BestBidPrice = tickList.Max(t => t.BestBidPrice), // use the highest bid price in the list
                        BestBidQuantity = tickList.Sum(t => t.BestBidQuantity), // sum up the bid quantities
                        Timestamp = tickList.Max(t => t.Timestamp), // use the latest timestamp in the list
                    };

                    tickList.Clear();

                    dbContext.KucoinTicks.Add(aggregateTick);
                    await dbContext.SaveChangesAsync(); // now we can do async operation
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
            finally
            {
                slimLock.Release();
            }
        }

        static async Task ReceivetickListAsync(KucoinSocketClient client)
        {
            // Subscribe to KuCoin websocket
            var subscribeResult = await client
                .SpotStreams.SubscribeToTickerUpdatesAsync("ETH-BTC", async data =>
                {
                    await slimLock.WaitAsync();
                    try
                    {
                        {
                            tickList.Add(data.Data);
                            //_socketSvc.HandleTickerStreamDataAsync(data, ct);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.ToString());
                    }
                    finally
                    {
                        slimLock.Release();
                    }
                });

            // Check if subscription was successful
            if (!subscribeResult.Success)
            {
                System.Console.WriteLine($"Failed to subscribe to ticker updates. Error: {subscribeResult.Error}");
            }
        }
    }

    class TimerState
    {
        public string TradingPair { get; set; }
    }
}