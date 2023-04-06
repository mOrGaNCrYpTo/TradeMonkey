using Kucoin.Net.Objects.Models.Spot.Socket;

using Mapster;

using TradeMonkey.Core;

namespace TradeMonkey.Trader
{
    public static class Program
    {
        private static CancellationTokenSource cts = new CancellationTokenSource();
        private static List<KucoinStreamTick> tickList = new();
        private static KucoinSocketSvc? _socketSvc;
        private static CancellationTokenSource _cts = new CancellationTokenSource();
        private static DomainConfiguration _config;

        static async Task Main()
        {
            TradeMonkeyConfigurationBuilder _configurationBuilder = new("");
            _config = _configurationBuilder.DomainConfiguration;

            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            await RunAsync(serviceProvider, _cts.Token);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JsonSerializerOptions>(options =>
            {
                options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.PropertyNameCaseInsensitive = true;
            });

            services.AddDbContext<TmDBContext>(options =>
                options.UseSqlServer(_config.DatabaseSettings.ConnectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );

            services.AddScoped<KuCoinDbRepository>();
            services.AddScoped<KucoinClient>();
            services.AddScoped<KucoinSocketClient>();
            services.AddScoped<KucoinTickerSvc>();
            services.AddScoped<TokenMetricsSvc>();

            var apiCredentials = (KucoinApiCredentials)_config.KucoinApi.Adapt<CryptoExchange.Net.Authentication.ApiCredentials>();

            services.AddKucoin((restClientOptions, socketClientOptions) =>
            {
                restClientOptions.ApiCredentials = apiCredentials;
                restClientOptions.LogLevel = LogLevel.Trace;
                socketClientOptions.ApiCredentials = apiCredentials;
                socketClientOptions.SpotStreamsOptions.AutoReconnect = true;
                socketClientOptions.SpotStreamsOptions.ReconnectInterval = TimeSpan.FromSeconds(10);
                socketClientOptions.SpotStreamsOptions.SocketResponseTimeout = TimeSpan.FromSeconds(10);
            });

            services.AddSingleton(provider =>
            {
                var uriBuilder = new UriBuilder
                {
                    Scheme = "https",
                    Host = _config.TokenMetricsApi.ApiBaseUrl
                };
                return uriBuilder;
            });

            services.AddHttpClient<TokenMetricsApiRepository>(httpClient =>
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Trade.Monkey");
                httpClient.DefaultRequestHeaders.Add(
                    _config.TokenMetricsApi.ApiKeyName,
                    _config.TokenMetricsApi.ApiKeyVal);
                httpClient.Timeout = TimeSpan.FromMinutes(10);
            })
                .SetHandlerLifetime(TimeSpan.FromMinutes(10));

            services.ScanCurrentAssembly(ServiceDescriptorMergeStrategy.TryAdd);
        }

        static async Task RunAsync(ServiceProvider serviceProvider, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            // Resolve clients
            var kucoinClient = serviceProvider.GetRequiredService<KucoinClient>();
            var kucoinSocketClient = serviceProvider.GetRequiredService<KucoinSocketClient>();

            // Resolve Services
            var kucoinAccountSvc = serviceProvider.GetRequiredService<KucoinAccountSvc>();
            var kucoinSocketSvc = serviceProvider.GetRequiredService<KucoinSocketSvc>();
            var kucoinTickerSvc = serviceProvider.GetRequiredService<KucoinTickerSvc>();
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
            //var state = new TimerState { TradingPair = "ETH-BTC" };
            //var timer = new Timer((s) => Task.Run(() => OnKucoinStreamTimerElapsed(s, dbContext)), state, 0, 300000);

            // Ticker Websocket subscription
            //await ReceivetickListAsync(kucoinSocketClient);

            // Get the symbols of the coins you're holding

            var accountBalances = await kucoinAccountSvc.GetAccountsAsync(null, null, ct);
            var assets = accountBalances
                .Where(s => s.Holds > 0)
                .Select(s => s.Asset)
                .ToList();

            // Define the time range for historical data
            var endTime = DateTime.UtcNow;
            var startTime = endTime.AddYears(-1);

            // ******* END KUCOIN TIMERS ******* //

            // ******* TOKEN METRICS TIMERS ******* //
            var batch = new List<int> { 3375, 3306, 3379 };
            var symbols = new List<string> { "ETH", "BTC" };

            // Now get the trader grades for the top tokens
            DateTime currentDateTime = DateTime.Now;
            var startDate = DateOnly.FromDateTime(currentDateTime.AddDays(-1));
            var endDate = DateOnly.FromDateTime(currentDateTime);
            var limit = 1000000;

            var tokenMetricsPrices = await tokenMetricsSvc.GetPricesAsync(batch, startDate, endDate, limit, cts.Token);
            var tokenMetricsGrades = await tokenMetricsSvc.GetTraderGradesAsync(batch, startDate, endDate, limit, cts.Token);
            //var tokenMetricsIndicator = await tokenMetricsSvc.GetIndicatorAsync(symbols, startDate, endDate, limit, ct);
            var tokenMetricsResistanceSupport = await tokenMetricsSvc.GetResistanceSupportAsync(symbols, startDate, endDate, limit, cts.Token);

            // ******* END TOKEN METRICS TIMERS ******* //

            // KEEP THE CONSOLE RUNNING BY WAITING FOR DATA INDEFINITELY
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        //private static async Task GetHistoricalDataAsync(KucoinTickerSvc kucoinTickerSvc, List<string> symbols,
        //    DateTime startTime, DateTime endTime)
        //{
        //    foreach (var symbol in symbols)
        //    {
        //        var klines = await kucoinClient.GetKlinesAsync(symbol, Kucoin.Net.Objects.KucoinKlineInterval.OneDay, startTime, endTime);

        //        if (klines.Success)
        //        {
        //            // Save klines.Data to the database ...
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Error fetching historical data for {symbol}: {klines.Error?.Message}");
        //        }
        //    }
        //}

        static void OnKucoinStreamTimerElapsed(object state, TmDBContext dbContext)
        {
            Console.WriteLine("Elapsed..");

            TimerState timerState = (TimerState)state;
            string tradingPair = timerState.TradingPair;

            if (tickList.Any())
            {
                var aggregateTick = new KucoinTick
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