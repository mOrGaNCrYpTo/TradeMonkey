using System.Reflection;

using TradeMonkey.Services.Interface;

namespace TradeMonkey.Trader
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
            Program program = new Program();
            await program.InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            await RunAsync(serviceProvider, _cts.Token);
        }

        private void ConfigureServices(IServiceCollection services)
        {
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

            services.ScanAssembly(Assembly.GetAssembly(typeof(ITraderService));
        }

        private async Task RunAsync(IServiceProvider serviceProvider, CancellationToken ct)
        {
            var kucoinSocketClient = serviceProvider.GetService<KucoinSocketClient>();
            var kucoinAccountSvc = serviceProvider.GetRequiredService<KucoinAccountSvc>();
            var kucoinTickerSvc = serviceProvider.GetRequiredService<KucoinTickerSvc>();
            var socketSvc = serviceProvider.GetRequiredService<KucoinSocketSvc>();
            var tokenMetricsSvc = serviceProvider.GetRequiredService<TokenMetricsSvc>();

            var dbContext = serviceProvider.GetRequiredService<TmDBContext>();
            await BackFillTokenMetricsDataAsync(tokenMetricsSvc, kucoinAccountSvc, ct);

            //await SetupTasks(kucoinSocketClient, tokenMetricsSvc, ct);
        }

        async Task SetupTasks(KucoinSocketClient kucoinSocketClient, TokenMetricsSvc tokenMetricsSvc, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            DateTime currentUtcTime = DateTime.UtcNow;

            DateTime targetTime = TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, targetTimeZone);

            TimeSpan timeUntilMidnight = TimeSpan.FromDays(1) - targetTime.TimeOfDay;

            var timerSettings = _config.TimerSettings;

            var kucoinTask = OnKucoinStreamTaskAsync(TimeSpan.FromSeconds(timerSettings.KucoinStreamInterval), ct);
            var tokenMetricsTask = OnTokenMetricsPricesTaskAsync(tokenMetricsSvc, TimeSpan.FromMinutes(timerSettings.TokenMetricsPricesInterval), ct);

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

        async Task BackFillTokenMetricsDataAsync(TokenMetricsSvc tokenMetricsSvc, KucoinAccountSvc kucoinAccountSvc, CancellationToken ct)
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
    }
}