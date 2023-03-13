using Kucoin.Net;
using Kucoin.Net.Objects.Models.Spot.Socket;

using System.Collections.Generic;

using TradeMonkey.Services;
using TradeMonkey.Trader.Services;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TradeMonkey.Trader
{
    public static class Program
    {
        private static List<KucoinStreamTick> tickList = new List<KucoinStreamTick>();
        private static KucoinSocketSvc socketSvc;
        private static CancellationTokenSource cts = new CancellationTokenSource();

        static async Task Main()
        {
            // Create a new instance of ServiceCollection
            var services = new ServiceCollection();

            services.Configure<JsonSerializerOptions>(options =>
            {
                options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.PropertyNameCaseInsensitive = true;
            });

            var credentials = new KucoinApiCredentials("63f3a3999ba1f40001e8c1a0",
                "3abfb8ef-498e-43a7-8d8c-b500fdea0991", "89t@UzifA$Hb6p5");

            services.AddDbContext<TmDBContext>(options =>
                options.UseSqlServer(Settings.TradeMonkeyDb)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );

            services.AddScoped<KucoinClient>();
            services.AddScoped<KucoinSocketClient>();

            services.AddKucoin((restClientOptions, socketClientOptions) =>
            {
                restClientOptions.ApiCredentials = credentials;
                restClientOptions.LogLevel = LogLevel.Trace;
                socketClientOptions.ApiCredentials = credentials;
                socketClientOptions.SpotStreamsOptions.AutoReconnect = true;
                socketClientOptions.SpotStreamsOptions.ReconnectInterval = TimeSpan.FromSeconds(10);
                socketClientOptions.SpotStreamsOptions.SocketResponseTimeout = TimeSpan.FromSeconds(10);
            });

            // Add UriBuilder as a singleton service
            services.AddScoped(provider =>
            {
                var uriBuilder = new UriBuilder
                {
                    Scheme = "https",
                    Host = Settings.TokenMetricsApiBaseUrl
                };
                return uriBuilder;
            });

            services.AddHttpClient<TokenMetricsApiRepository>(httpClient =>
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Trade.Monkey");
                httpClient.DefaultRequestHeaders.Add(Settings.TokenMetricsApiKeyName, Settings.TokenMetricsApiKeyVal);
                httpClient.Timeout = TimeSpan.FromMinutes(10);
            })
                .SetHandlerLifetime(TimeSpan.FromMinutes(10));

            services.ScanCurrentAssembly(ServiceDescriptorMergeStrategy.TryAdd);

            // Create a new instance of ServiceProvider
            var serviceProvider = services.BuildServiceProvider();

            // Resolve your services from the ServiceProvider
            var kucoinTickerSvc = serviceProvider.GetRequiredService<KucoinTickerSvc>();
            socketSvc = serviceProvider.GetRequiredService<KucoinSocketSvc>();
            var kucoinSocketClient = serviceProvider.GetRequiredService<KucoinSocketClient>();

            var tokenMetricsSvc = serviceProvider.GetRequiredService<TokenMetricsSvc>();
            //await tokenMetricsSvc.GetAllTokens(ct);

            // Set the target timezone
            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            // Get the current UTC time
            DateTime currentUtcTime = DateTime.UtcNow;

            // Convert to the target timezone
            DateTime targetTime = TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, targetTimeZone);

            // Calculate the time until the next midnight in the target timezone
            TimeSpan timeUntilMidnight = TimeSpan.FromDays(1) - targetTime.TimeOfDay;

            var dbContext = serviceProvider.GetRequiredService<TmDBContext>();

            // ******* KUCOIN TIMERS ******* //

            // Stream aggregation process every -Default 5 minutes (300000 ms)
            var state = new TimerState { TradingPair = "ETH-BTC" };
            var timer = new Timer((s) => Task.Run(() => OnKucoinStreamTimerElapsed(s, dbContext)), state, 0, 300000);

            await ReceivetickListAsync(kucoinSocketClient);

            // ******* END KUCOIN TIMERS ******* //

            // ******* TOKEN METRICS TIMERS ******* //
            var batch = new List<int> { 3375, 3306, 3379 };
            var symbols = new List<string> { "ETH", "BTC" };

            // Now get the trader grades for the top tokens
            DateTime dateTime = DateTime.Now;
            var startDate = dateTime.AddDays(-1).ToString("yyyy-MM-dd");
            var endDate = dateTime.ToString("yyyy-MM-dd");
            var limit = 1000000;

            var tokenMetricsPrices = await tokenMetricsSvc.GetPricesAsync(batch, startDate, endDate, limit, ct);
            var tokenMetricsGrades = await tokenMetricsSvc.GetTraderGradesAsync(batch, startDate, endDate, limit, ct);
            var tokenMetricsIndicator = await tokenMetricsSvc.GetIndicatorAsync(symbols, startDate, endDate, limit, ct);
            var tokenMetricsResistanceSupport = await tokenMetricsSvc.GetResistanceSupportAsync(symbols, startDate, endDate, limit, ct);

            // ******* END TOKEN METRICS TIMERS ******* //

            // KEEP THE CONSOLE RUNNING BY WAITING FOR DATA INDEFINITELY
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        static void OnKucoinStreamTimerElapsed(object state, TmDBContext dbContext)
        {
            Console.WriteLine("Elapsed..");

            TimerState timerState = (TimerState)state;
            string tradingPair = timerState.TradingPair;

            if (tickList.Any())
            {
                var aggregateTick = new Data.Entity.KucoinTick
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
                dbContext.SaveChanges();
            };
        }

        static async Task ReceivetickListAsync(KucoinSocketClient client)
        {
            // Subscribe to KuCoin websocket
            var subscribeResult = await client
                .SpotStreams.SubscribeToTickerUpdatesAsync("ETH-BTC", async data =>
                {
                    //Console.WriteLine("Got data");
                    tickList.Add(data.Data);
                    //await kucoinSocketSvc.HandleTickerStreamDataAsync(data, ct);
                });
        }
    }

    class TimerState
    {
        public string TradingPair { get; set; }
    }
}