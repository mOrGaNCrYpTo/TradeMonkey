using Kucoin.Net;

using TradeMonkey.Trader.Services;

namespace TradeMonkey.Trader
{
    public static class Program
    {
        static async Task Main()
        {
            var ct = new CancellationToken();
            ct.ThrowIfCancellationRequested();

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
            );

            services.AddScoped<KucoinClient>();

            services.AddKucoin((restClientOptions, socketClientOptions) =>
            {
                restClientOptions.ApiCredentials = credentials;
                restClientOptions.LogLevel = LogLevel.Trace;
                socketClientOptions.ApiCredentials = credentials;
            });

            // Add UriBuilder as a singleton service
            services.AddScoped(provider =>
            {
                var uriBuilder = new UriBuilder
                {
                    Scheme = "https",
                    Host = Settings.TokenMetricsApiBaseUrl,
                    //Port = 443
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

            var tokenMetricsSvc = serviceProvider.GetRequiredService<TokenMetricsSvc>();
            //await tokenMetricsSvc.GetAllTokens(ct);

            // Set the target timezone
            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

            // Get the current UTC time
            DateTime currentUtcTime = DateTime.UtcNow;

            // Convert to the target timezone
            DateTime targetTime = TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, targetTimeZone);

            // Calculate the time until the next midnight in the target timezone
            TimeSpan timeUntilMidnight = TimeSpan.FromDays(1) - targetTime.TimeOfDay;

            //SET UP A TIMERS TO CALL THE API ENDPOINTS AT PLANNED INTERVALS
            var apiTimer = new Timer(async (state) =>
            {
                // Get the latest ticker data from the KuCoin API for tickers
                var symbols = await kucoinTickerSvc.GetTopTokensAsync(1000000, 0.5, 20, ct);
            }, null, timeUntilMidnight, TimeSpan.FromDays(1));

            // Future me. Add an endpoint to get these
            List<int> symbols = new()
            {
                2974,3119,2974,3119,3306,3312,3315,3369,3375,3415,3924,3988,4015,14934,17659,565,1892,3600,4462,11642,11814,12631,15595,16200,17010,20420,22067,24472,24529
            };

            // Now get the trader grades for the top tokens
            DateTime dateTime = DateTime.Now;
            var startDate = dateTime.AddDays(-1).ToString("yyyy-MM-dd");
            var endDate = dateTime.ToString("yyyy-MM-dd");
            var limit = 1000000;

            for (int i = 0; i < symbols.Count; i += 5)
            {
                List<int> batch = symbols.GetRange(i, Math.Min(5, symbols.Count - i));
                Console.WriteLine("Processing batch: " + string.Join(",", batch));

                startDate = dateTime.AddDays(-90).ToString("yyyy-MM-dd");
                var tokenMetricsPrices = await tokenMetricsSvc.GetPricesAsync(symbols, startDate, endDate, limit, ct);
            }

            //var tokenMetricsIndicator = await tokenMetricsSvc.GetIndicatorAsync(symbols, startDate, endDate, limit, ct);
            //var tokenMetricsResistanceSupport = await tokenMetricsSvc.GetResistanceSupportAsync(symbols, startDate, endDate, limit, ct);
        }
    }
}