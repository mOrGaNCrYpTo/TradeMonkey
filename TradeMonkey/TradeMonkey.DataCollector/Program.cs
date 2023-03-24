using Mapster;

namespace TradeMonkey.Trader
{
    public static class Program
    {
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

            var apiCredentials = (Kucoin.Net.Objects.KucoinApiCredentials)_config.KucoinApiCredentials.Adapt<CryptoExchange.Net.Authentication.ApiCredentials>();

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

        private static async Task RunAsync(IServiceProvider serviceProvider, CancellationToken ct)
        {
            var kucoinTickerSvc = serviceProvider.GetRequiredService<KucoinTickerSvc>();
            var socketSvc = serviceProvider.GetRequiredService<KucoinSocketSvc>();
            var kucoinSocketClient = serviceProvider.GetRequiredService<KucoinSocketClient>();
            var tokenMetricsSvc = serviceProvider.GetRequiredService<TokenMetricsSvc>();

            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            DateTime currentUtcTime = DateTime.UtcNow;

            DateTime targetTime = TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, targetTimeZone);

            TimeSpan timeUntilMidnight = TimeSpan.FromDays(1) - targetTime.TimeOfDay;

            var dbContext = serviceProvider.GetRequiredService<TmDBContext>();

            await BackFillPriceAndSRDataAsync(tokenMetricsSvc, ct);

            //var timerSettings = _config.TimerSettings;

            //var state = new TimerState { TradingPair = "ETH-BTC" };
            //var kucoinTimer = new Timer((s) => Task.Run(() =>
            //    OnKucoinStreamTimerElapsed(s, dbContext)), state, 0, timerSettings.KucoinStreamInterval);

            //await ReceivetickListAsync(kucoinSocketClient);

            // ******* END KUCOIN TIMERS ******* //

            // ******* TOKEN METRICS TIMERS ******* //

            //var tokenMetricsTimer =
            //    new Timer((s) => Task.Run(() =>
            //            OnTokenMetricsPricesTimerElapsed(tokenMetricsSvc, _cts.Token)), state, 0,
            //                timerSettings.TokenMetricsPricesInterval);

            // ******* END TOKEN METRICS TIMERS ******* //

            // KEEP THE CONSOLE RUNNING BY WAITING FOR DATA INDEFINITELY
            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), ct);
            }
        }

        static async Task OnTokenMetricsPricesTimerElapsed(TokenMetricsSvc tokenMetricsSvc, CancellationToken ct)
        {
            var batch = new List<int> { 3375, 3306 };
            var symbols = new List<string> { "ETH", "BTC" };
            var limit = 1000000;

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly oneYearAgo = currentDate.AddDays(-1);

            DateOnly startDate = currentDate.AddDays(-5);
            DateOnly endDate = currentDate;

            List<Task> tasks = new()
            {
                tokenMetricsSvc.GetPricesAsync(batch, startDate, endDate, limit, ct),
                tokenMetricsSvc.GetResistanceSupportAsync(symbols, startDate, endDate, limit, ct)
            };

            await Task.WhenAll(tasks);
        }

        static async Task BackFillPriceAndSRDataAsync(TokenMetricsSvc tokenMetricsSvc, CancellationToken ct)
        {
            KucoinTickerSvc

            var batch = new List<int> { 3375, 3306 };
            var symbols = new List<string> { "ETH", "BTC" };
            var limit = 1000000;

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly oneYearAgo = currentDate.AddYears(-1);

            List<Task> tasks = new();

            DateOnly startDate = currentDate.AddDays(-5);
            DateOnly endDate = currentDate;

            tasks.Add(tokenMetricsSvc.GetResistanceSupportAsync(symbols, startDate, endDate, limit, ct));

            while (oneYearAgo <= currentDate)
            {
                startDate = oneYearAgo;
                endDate = oneYearAgo.AddMonths(1);

                Console.WriteLine($"Getting token metrics prices for {startDate} to {endDate}");

                tasks.Add(tokenMetricsSvc.GetPricesAsync(batch, startDate, endDate, limit, ct));

                // Move to the next month
                oneYearAgo = endDate;
            }

            await Task.WhenAll(tasks);
        }

        static async Task OnTokenMetricsGradesTimerElapsed(TokenMetricsSvc tokenMetricsSvc, CancellationToken ct)
        {
            var batch = new List<int> { 3375, 3306 };
            var symbols = new List<string> { "ETH", "BTC" };

            DateTime currentDateTime = DateTime.Now;
            var startDate = DateOnly.FromDateTime(currentDateTime.AddDays(-1));
            var endDate = DateOnly.FromDateTime(currentDateTime);
            var limit = 1000000;

            List<Task> tasks = new()
            {
                tokenMetricsSvc.GetTraderGradesAsync(batch, startDate, endDate, limit, ct),
                tokenMetricsSvc.GetMarketIndicatorAsync(symbols, startDate, endDate, limit, ct),
            };

            await Task.WhenAll(tasks);
        }

        static void OnKucoinStreamTimerElapsed(object state, TmDBContext dbContext)
        {
            Console.WriteLine("Kucoin Stream Timer Elapsed..");

            TimerState timerState = (TimerState)state;
            string tradingPair = timerState.TradingPair;

            if (tickList.Any())
            {
                var aggregateTick = new Data.Entity.KucoinTick
                {
                    Sequence = tickList.Max(t => t.Sequence),
                    LastPrice = tickList.Average(t => t.LastPrice),
                    LastQuantity = tickList.Sum(t => t.LastQuantity),
                    BestAskPrice = tickList.Min(t => t.BestAskPrice),
                    BestAskQuantity = tickList.Sum(t => t.BestAskQuantity),
                    BestBidPrice = tickList.Max(t => t.BestBidPrice),
                    BestBidQuantity = tickList.Sum(t => t.BestBidQuantity),
                    Timestamp = tickList.Max(t => t.Timestamp),
                };

                tickList.Clear();

                dbContext.KucoinTicks.Add(aggregateTick);
                dbContext.SaveChanges();
            };
        }

        static async Task ReceivetickListAsync(KucoinSocketClient client)
        {
            var subscribeResult = await client
                .SpotStreams.SubscribeToTickerUpdatesAsync("ETH-BTC", async data =>
                {
                    tickList.Add(data.Data);
                });
        }
    }

    record TimerState
    {
        public string TradingPair { get; init; }
    }
}