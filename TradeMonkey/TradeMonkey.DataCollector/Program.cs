using TradeMonkey.DataCollector.Services;

public static class Program
{
    static async Task Main()
    {
        // Step 2: Create a new instance of ServiceCollection
        var services = new ServiceCollection();

        // Step 3: Register your services with the container
        services.ScanCurrentAssembly();

        services.AddDbContextPool<TmDBContext>(options =>
            options.UseSqlServer("Data Source=HP\\MFSQL;Initial Catalog=TradeMonkey;Integrated Security=True"));

        services.AddKucoin((o, so) =>
        {
            KucoinClientOptions options = new()
            {
                LogLevel = LogLevel.Debug,
                ApiCredentials =
                    new KucoinApiCredentials("63f3a3999ba1f40001e8c1a0", "3abfb8ef-498e-43a7-8d8c-b500fdea0991", "89t@UzifA$Hb6p5")
            };
            o.ApiCredentials = options.ApiCredentials;
            o.LogLevel = options.LogLevel;

            // If you want to modify the socket client options as well, you can do so here:
            //so.Url = "wss://some-other-url";
        });

        // Step 4: Create a new instance of ServiceProvider
        var serviceProvider = services.BuildServiceProvider();

        // Step 5: Resolve your services from the ServiceProvider
        var kucoinWebsocketService = serviceProvider.GetService<KucoinWebsocketService>();

        CancellationToken token = new CancellationToken();

        // Use the service
        await kucoinWebsocketService.StartSubscription(token);

        //_client = new KucoinSocketClient(options);
        //await SubscribeToAllTickerUpdatesAsync(ct);
    }
}