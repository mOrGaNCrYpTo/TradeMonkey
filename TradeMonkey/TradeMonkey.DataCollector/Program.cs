using Microsoft.Extensions.Configuration;

namespace TradeMonkey.Trader
{
    public static class Program
    {
        private static List<KucoinStreamTick> tickList = new();
        private static KucoinSocketSvc? socketSvc;
        private static CancellationTokenSource cts = new CancellationTokenSource();

        static async Task Main()
        {
            var configuration = LoadConfiguration();

            // Create a new instance of ServiceCollection
            var services = new ServiceCollection();
            ConfigureServices(services, configuration);

            // Create a new instance of ServiceProvider
            var serviceProvider = services.BuildServiceProvider();

            // Resolve your services from the ServiceProvider
            await RunAsync(serviceProvider, cts.Token);
        }

        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            return builder.Build();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JsonSerializerOptions>(options =>
            {
                options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.PropertyNameCaseInsensitive = true;
            });

            var credentials = configuration.GetSection("ApiSettings:KucoinApiCredentials").Get<KucoinApiCredentials>();

            services.AddDbContext<TmDBContext>(options =>
                options.UseSqlServer(configuration["ApiSettings:TradeMonkeyDb"])
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

            services.AddSingleton(provider =>
            {
                var uriBuilder = new UriBuilder
                {
                    Scheme = "https",
                    Host = configuration["ApiSettings:TokenMetricsApi:BaseUrl"]
                };
                return uriBuilder;
            });

            services.AddHttpClient<TokenMetricsApiRepository>(httpClient =>
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Trade.Monkey");
                httpClient.DefaultRequestHeaders.Add(configuration["ApiSettings:TokenMetricsApi:ApiKeyName"], configuration["ApiSettings:TokenMetricsApi:ApiKeyVal"]);
                httpClient.Timeout = TimeSpan.FromMinutes(10);
            })
                .SetHandlerLifetime(TimeSpan.FromMinutes(10));

            services.ScanCurrentAssembly(ServiceDescriptorMergeStrategy.TryAdd);
        }

        private static async Task RunAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var kucoinTickerSvc = serviceProvider.GetRequiredService<KucoinTickerSvc>();
            socketSvc = serviceProvider.GetRequiredService<KucoinSocketSvc>();
            var kucoinSocketClient = serviceProvider.GetRequiredService<KucoinSocketClient>();

            var tokenMetricsSvc = serviceProvider.GetRequiredService<TokenMetricsSvc>();

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

            // Stream aggregation process every - Default 5 minutes (300000 ms)
            var timerSettings = serviceProvider.GetRequiredService<IConfiguration>().GetSection("TimerSettings");
            int kucoinStreamInterval = timerSettings.GetValue<int>("KucoinStreamInterval");

            var state = new TimerState { TradingPair = "ETH-BTC" };
            var timer = new Timer((s) => Task.Run(() => OnKucoinStreamTimerElapsed(s, dbContext)), state, 0, kucoinStreamInterval);

            await ReceivetickListAsync(kucoinSocketClient);

            // ******* END KUCOIN TIMERS ******* //

            // ******* TOKEN METRICS TIMERS ******* //
            var batch = new List<int> { 3375, 3306 };
            var symbols = new List<string> { "ETH", "BTC" };

            // USDT = 3379

            // Now get the trader grades for the top tokens
            DateTime dateTime = DateTime.Now;
            var startDate = dateTime.AddDays(-1).ToString("yyyy-MM-dd");
            var endDate = dateTime.ToString("yyyy-MM-dd");
            var limit = 1000000;

            List<Task> tasks = new()
            {
                tokenMetricsSvc.GetPricesAsync(batch, startDate, endDate, limit, cancellationToken),
                tokenMetricsSvc.GetTraderGradesAsync(batch, startDate, endDate, limit, cancellationToken),
                tokenMetricsSvc.GetMarketIndicatorAsync(symbols, startDate, endDate, limit, cancellationToken),
                tokenMetricsSvc.GetResistanceSupportAsync(symbols, startDate, endDate, limit, cancellationToken)
            };

            await Task.WhenAll(tasks);

            // ******* END TOKEN METRICS TIMERS ******* //

            // KEEP THE CONSOLE RUNNING BY WAITING FOR DATA INDEFINITELY
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
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

    record TimerState
    {
        public string? TradingPair { get; init; }
    }
}