﻿using TradeMonkey.Core.Value.Constant;
using TradeMonkey.Services.Service;

namespace TradeMonkey.DataCollector
{
    public class Program
    {
        private readonly List<KucoinStreamTick> tickList = new();
        private readonly CancellationTokenSource _cts = new();
        private readonly DomainConfiguration _config;

        public Program()
        {
            TradeMonkeyConfigurationBuilder _configurationBuilder = new("");
            _config = _configurationBuilder.DomainConfiguration;
        }

        public static async Task Main()
        {
            Program program = new();
            await program.InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            // Load required assemblies explicitly
            Assembly.Load("TradeMonkey.Core");
            Assembly.Load("TradeMonkey.Data");
            Assembly.Load("TradeMonkey.Services");

            var services = new ServiceCollection();

            // Get all the assemblies that start with "TradeMonkey"
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith("TradeMonkey"));

            // Scan and register all the types in the assemblies
            foreach (var assembly in assemblies)
            {
                Console.WriteLine(assembly.FullName);
                services.ScanAssembly(assembly, type => true);
            }

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

            services.AddDbContext<TmDBContext>();

            KucoinApiCredentials apiCredentials = new(_config.KucoinApi.ApiKey,
                                                      _config.KucoinApi.ApiSecret,
                                                      _config.KucoinApi.ApiPassphrase);

            KucoinClient.SetDefaultOptions(new KucoinClientOptions
            {
                ApiCredentials = apiCredentials,
                LogLevel = LogLevel.Trace
            });

            services.AddScoped<KucoinClient>();

            KucoinSocketClient.SetDefaultOptions(new KucoinSocketClientOptions
            {
                ApiCredentials = apiCredentials,
                LogLevel = LogLevel.Trace
            });

            services.AddScoped<CoinApiService>();
            services.AddScoped<KucoinSocketClient>();
            services.AddScoped<CoinApiRestClient>();

            //services.AddScoped<KucoinAccountSvc>();
            //services.AddScoped<KuCoinDbRepository>();
            //services.AddScoped<KucoinSocketSvc>();
            services.AddScoped<KucoinTickerSvc>();

            //services.AddScoped<TokenMetricsSvc>();

            services.AddSingleton(provider =>
            {
                var uriBuilder = new UriBuilder
                {
                    Scheme = "https",
                    Host = _config.TokenMetricsApi.ApiBaseUrl
                };
                return uriBuilder;
            });

            services.AddHttpClient<CoinApiService>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(_config.CoinApi.ApiBaseUrl);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Invoices.Function");
                httpClient.DefaultRequestHeaders.Add(_config.CoinApi.ApiKeyName, _config.CoinApi.ApiKeyValue);
            })
                 .SetHandlerLifetime(TimeSpan.FromMinutes(5));

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
        }

        /// <summary>
        /// Use GetService method when you need to handle the case where the requested service
        /// cannot be resolved, and GetRequiredService method when you expect the requested service
        /// to always be present in the service provider.
        /// </summary>
        /// <param name="serviceProvider"> </param>
        /// <param name="ct">              </param>
        /// <returns> </returns>
        private async Task RunAsync(IServiceProvider serviceProvider, CancellationToken ct)
        {
            var kucoinTickerSvc = serviceProvider.GetService<KucoinTickerSvc>();
            var socketSvc = serviceProvider.GetService<KucoinSocketSvc>();
            var kucoinAccountSvc = serviceProvider.GetService<KucoinAccountSvc>();
            var kucoinSocketClient = serviceProvider.GetService<KucoinSocketClient>();
            var tokenMetricsSvc = serviceProvider.GetService<TokenMetricsSvc>();
            var dbContext = serviceProvider.GetService<TmDBContext>();

            CoinApiService coinApiService = serviceProvider.GetService<CoinApiService>();

            // This should only run once unless it fails
            await BackFillCoinApiDataAsync(coinApiService, kucoinAccountSvc, ct);

            // This should run every 5 minutes. It should collect and store updated data from
            // Kucoin, CoinApi, and TokenMetrics
            await SetupTasks(coinApiService, coinApiService, ct);
        }

        //async Task SetupTasks(CoinApiService coinApiService, CancellationToken ct)
        async Task SetupTasks(KucoinSocketClient tokenMetricsSvc, TokenMetricsSvc tokenMetricsSvc, TmDBContext dbContext, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            DateTime endDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetTimeZone);

            DateTime startDateTime = endDateTime.AddDays(-200);

            var formattedEndDateTime = endDateTime.ToString("yyyy-MM-ddTHH:mm:ss");
            var formattedStartDateTime = startDateTime.ToString("yyyy-MM-ddTHH:mm:ss");

            var timerSettings = _config.TimerSettings;

            var coinApiTask = GetCryptoData(coinApiService, formattedStartDateTime, formattedEndDateTime, ct);
            var kucoinTask = OnKucoinStreamTaskAsync(dbContext, TimeSpan.FromSeconds(timerSettings.KucoinStreamInterval), ct);
            var tokenMetricsTask = OnTokenMetricsPricesTaskAsync(tokenMetricsSvc, TimeSpan.FromMinutes(timerSettings.TokenMetricsPricesInterval), ct);

            await Task.WhenAll(coinApiTask);
        }

        async Task GetCryptoData(CoinApiService coinApiService, string start, string end, CancellationToken ct)
        {
            int limit = 100000;
            await coinApiService.GetCryptoData("5MIN", start, end, limit, ct);
        }

        async Task OnKucoinStreamTaskAsync(TmDBContext dbContext, TimeSpan interval, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(interval, ct);

                Console.WriteLine("Kucoin Stream Aggregate Task Triggered..");

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
        }

        async Task ReceivetickListAsync(KucoinSocketClient client)
        {
            var subscribeResult = await client
                .SpotStreams.SubscribeToTickerUpdatesAsync("ETH-BTC", async data =>
                {
                    tickList.Add(data.Data);
                });
        }

        async Task OnTokenMetricsPricesTaskAsync(TokenMetricsSvc tokenMetricsSvc, TimeSpan interval, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(interval, ct);

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
        }

        async Task BackFillDataAsync(CoinApiService coinApiService, TokenMetricsSvc tokenMetricsSvc,
            KucoinAccountSvc kucoinAccountSvc, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            // Get the assets currently held in my Kucoin account
            var accounts = await kucoinAccountSvc.GetAccountsAsync(null, null, ct);
            var assets = accounts
                .Where(s => s.Holds > 0)
                .Select(s => s.Asset)
                .ToList();

            // Get historical OHLCV data for each asset for the past 2 years. This will be used to
            // backfill the database with historical data that will be used to calculate the
            // indicators and training a model do this in batches to prevent timeouts This should
            // also retrieve other useful information from the APIs
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly oneYearAgo = currentDate.AddYears(-1);

            List<Task> tasks = new();

            DateOnly startDate = currentDate.AddDays(-5);
            DateOnly endDate = currentDate;
            int limit = 100000;

            List<int> tokenIds = await tokenMetricsSvc.GetTokenIdsByName(assets, ct);

            while (oneYearAgo <= currentDate)
            {
                startDate = oneYearAgo;
                endDate = oneYearAgo.AddMonths(1);

                Console.WriteLine($"Getting token metrics prices for {startDate} to {endDate}");

                tasks.Add(tokenMetricsSvc.GetResistanceSupportAsync(assets, startDate, endDate, limit, ct));
                tasks.Add(tokenMetricsSvc.GetMarketIndicatorAsync(assets, startDate, endDate, limit, ct));
                tasks.Add(tokenMetricsSvc.GetPricesAsync(tokenIds, startDate, endDate, limit, ct));

                // Move to the next month
                oneYearAgo = endDate;
            }

            await Task.WhenAll(tasks);
        }

        record TimerState
        {
            public string TradingPair { get; init; }
        }
    }
}