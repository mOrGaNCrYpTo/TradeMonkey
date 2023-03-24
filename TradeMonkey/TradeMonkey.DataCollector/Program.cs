using Kucoin.Net.Clients.SpotApi;
using Kucoin.Net.Objects;

using Mapster;

using TradeMonkey.Data.Entity;

namespace TradeMonkey.Trader
{
    public class Program
    {
        private List<KucoinStreamTick> tickList = new();
        private KucoinSocketSvc? _socketSvc;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private DomainConfiguration _config;

        public static async Task Main()
        {
            Program program = new Program();
            await program.InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            TradeMonkeyConfigurationBuilder _configurationBuilder = new("");
            _config = _configurationBuilder.DomainConfiguration;

            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            await RunAsync(serviceProvider, _cts.Token);
        }

        private void ConfigureServices(IServiceCollection services)
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

            var apiCredentials = (KucoinApiCredentials)_config.KucoinApiCredentials.Adapt<CryptoExchange.Net.Authentication.ApiCredentials>();

            services.AddKucoin((restClientOptions, socketClientOptions) =>
            {
                restClientOptions.ApiCredentials = apiCredentials;
                restClientOptions.LogLevel = LogLevel.Trace;
                socketClientOptions.ApiCredentials = apiCredentials;
                socketClientOptions.SpotStreamsOptions.AutoReconnect = true;
                socketClientOptions.SpotStreamsOptions.ReconnectInterval = TimeSpan.FromSeconds(10);
                socketClientOptions.SpotStreamsOptions.SocketResponseTimeout = TimeSpan.FromSeconds(10);
            });

            services.AddScoped<KucoinSocketSvc>();
            services.AddScoped<KucoinClientSpotApi>();
            services.AddScoped<KucoinClientSpotApiAccount>();

            services.AddScoped<KuCoinDbRepository>();
            services.AddScoped<KucoinAccountSvc>();
            services.AddScoped<KucoinTickerSvc>();
            services.AddScoped<TokenMetricsSvc>();

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

        private async Task RunAsync(IServiceProvider serviceProvider, CancellationToken ct)
        {
            var kucoinAccountSvc = serviceProvider.GetRequiredService<KucoinAccountSvc>();
            var kucoinTickerSvc = serviceProvider.GetRequiredService<KucoinTickerSvc>();
            var socketSvc = serviceProvider.GetRequiredService<KucoinSocketSvc>();
            var kucoinSocketClient = serviceProvider.GetRequiredService<KucoinSocketClient>();
            var tokenMetricsSvc = serviceProvider.GetRequiredService<TokenMetricsSvc>();

            var dbContext = serviceProvider.GetRequiredService<TmDBContext>();
            await BackFillTokenMetricsDataAsync(tokenMetricsSvc, kucoinAccountSvc, ct);

            //await SetupTasks();
        }

        async Task SetupTasks(KucoinSocketClient kucoinSocketClient, TokenMetricsSvc tokenMetricsSvc, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            DateTime currentUtcTime = DateTime.UtcNow;

            DateTime targetTime = TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, targetTimeZone);

            TimeSpan timeUntilMidnight = TimeSpan.FromDays(1) - targetTime.TimeOfDay;

            var timerSettings = _config.TimerSettings;

            // Run tasks in the background var kucoinTask =
            OnKucoinStreamTaskAsync(TimeSpan.FromSeconds(timerSettings.KucoinStreamInterval), ct); var
            tokenMetricsTask = OnTokenMetricsPricesTaskAsync(,
            TimeSpan.FromMinutes(timerSettings.TokenMetricsPricesInterval), ct);

            // Wait for all tasks to complete or be canceledtokenMetricsSvc
            await Task.WhenAll(kucoinTask, tokenMetricsTask);
        }

        async Task OnKucoinStreamTaskAsync(TimeSpan interval, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(interval, ct);

                Console.WriteLine("Kucoin Stream Task Triggered..");

                // ... The original logic from OnKucoinStreamTimerElapsed method ...
            }
        }

        async Task OnTokenMetricsPricesTaskAsync(TokenMetricsSvc tokenMetricsSvc, TimeSpan interval, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(interval, ct);

                var batch = new List<int> { 3375, 3306 }; var symbols = new List<string> { "ETH", "BTC" };
                var limit = 1000000;

                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now); DateOnly oneYearAgo = currentDate.AddDays(-1);

                DateOnly startDate = currentDate.AddDays(-5); DateOnly endDate = currentDate;

                List<Task> tasks = new()

                     tokenMetricsSvc.GetPricesAsync(batch, startDate, endDate, limit, ct),
                     tokenMetricsSvc.GetResistanceSupportAsync(symbols, startDate, endDate, limit, ct)

            await Task.WhenAll(tasks);
            }
        }

        async Task BackFillTokenMetricsDataAsync(TokenMetricsSvc tokenMetricsSvc, KucoinAccountSvc kucoinAccountSvc,
       CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var accountBalances = await kucoinAccountSvc.GetAccountsAsync(null, null, ct);
            var assets = accountBalances
                .Where(s => s.Holds > 0)
                .Select(s => s.Asset)
                .ToList();

            var tokenIds = tokenMetricsSvc.GetTokenIdsByName(assets, ct);

            var batch = new List<int> { 3375, 3306 };
            var symbols = new List<string> { "ETH", "BTC", "SOL", "FTM" };
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

        async Task<IEnumerable<Kucoin.Net.Objects.Models.Spot.KucoinAccount>> GetKucoinAccountData(KucoinAccountSvc svc, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            return await svc.GetAccountsAsync(null, null, ct);
        }

        async Task OnTokenMetricsGradesTimerElapsed(TokenMetricsSvc tokenMetricsSvc, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

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

        void OnKucoinStreamTimerElapsed(object state, TmDBContext dbContext, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Console.WriteLine("Kucoin Stream Timer Elapsed..");

            TimerState timerState = (TimerState)state;
            string tradingPair = timerState.TradingPair;

            if (tickList.Any())
            {
                var aggregateTick = new KucoinTick
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

        async Task ReceivetickListAsync(KucoinSocketClient client)
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