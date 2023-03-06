using Kucoin.Net;

using TradeMonkey.DecisionData.Services;

namespace TradeMonkey.DecisionData
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
                options.ReferenceHandler = ReferenceHandler.Preserve;
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

            services.AddHttpClient<TokenMetricsApiRepository>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(Settings.TokenMetricsApiBaseUrl);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Trade.Monkey");
                httpClient.DefaultRequestHeaders.Add(Settings.TokenMetricsApiKeyName, Settings.TokenMetricsApiKeyVal);
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

            // SET UP A TIMERS TO CALL THE API ENDPOINTS AT PLANNED INTERVALS
            //var apiTimer = new Timer(async (state) =>
            //{
            // Get the latest ticker data from the KuCoin API for tickers SAVING THIS FOR
            // GETTTING THE TOP COINS. GOING TO USE A STATIC LIST FOR NOW await kucoinTickerSvc.GetLatestTickerDataAsync(ct);
            //var symbols = await kucoinTickerSvc.GetTopTokensAsync(1000000, 0.5, 20, ct);

            // Future me. Add an endpoint to get these
            List<int> symbols = new()
                {
                    2974,3119,2974,3119,3306,3312,3315,3369,3375,3415,3924,3988,4015,14934
                };

            // Now get the trader grades for the top tokens
            DateTime dateTime = DateTime.Now;
            var startDate = dateTime.AddDays(-90).ToString("yyyy-MM-dd");
            var endDate = dateTime.ToString("yyyy-MM-dd");
            var limit = 100000;

            var tokenMetricsGrades = await tokenMetricsSvc.GetTraderGradesAsync(symbols, startDate, endDate, limit, ct);
            var tokenMetricsPrices = await tokenMetricsSvc.GetPricesAsync(symbols, startDate, endDate, limit, ct);
            //var tokenMetricsIndicator = await tokenMetricsSvc.GetIndicatorAsync(symbols, startDate, endDate, limit, ct);
            //var tokenMetricsResistanceSupport = await tokenMetricsSvc.GetResistanceSupportAsync(symbols, startDate, endDate, limit, ct);
            //}, null, timeUntilMidnight, TimeSpan.FromDays(1));
        }
    }
}