using TradeMonkey.DataCollector.Services;
using TradeMonkey.DataCollector.Value.Response;

public static class Program
{
    static async Task Main()
    {
        var services = new ServiceCollection();
        services.ScanCurrentAssembly();

        services.AddDbContext<TmDBContext>();

        var credentials = new KucoinApiCredentials("63f3a3999ba1f40001e8c1a0",
            "3abfb8ef-498e-43a7-8d8c-b500fdea0991", "89t@UzifA$Hb6p5");

        KucoinClient.SetDefaultOptions(new KucoinClientOptions
        {
            LogLevel = LogLevel.Debug,
            ApiCredentials = credentials
        });

        KucoinSocketClient.SetDefaultOptions(new KucoinSocketClientOptions
        {
            ApiCredentials = credentials
        });

        // Create a new instance of ServiceProvider
        var serviceProvider = services.BuildServiceProvider();

        // Resolve your services from the ServiceProvider
        var kucoinTickerSvc = serviceProvider.GetService<KucoinTickerSvc>();

        var ct = new CancellationToken();

        ct.ThrowIfCancellationRequested();

        // Use your service
        await kucoinTickerSvc.GetLatestTickerDataAsync(ct);

        //_client = new KucoinSocketClient(options);
        //await SubscribeToAllTickerUpdatesAsync(ct);
    }

    static async Task UpdateAccountsAsync()
    {
        //var accounts = await _apiRepository.GetAccountsAsync(ct);

        //if (accounts.Success)
        //{
        //    foreach (var account in accounts.Data)
        //    {
        //        Console.WriteLine(account.Asset);
        //        Console.WriteLine(account.Available);
        //    }
        //}
    }

    //static async Task SubscribeToAllTickerUpdatesAsync(CancellationToken ct)
    //{
    //    try
    //    {
    //        var result = await _client.SpotStreams.SubscribeToAllTickerUpdatesAsync(async (DataEvent<KucoinStreamTick> dataEvent) =>
    //        {
    //            // Handle the new ticker data here
    //            KucoinStreamTick tick = dataEvent.Data;
    //            Console.WriteLine($"Received new ticker data for {tick.Symbol}: last price = {tick.LastPrice}");

    // await KucoinTickerDataSvc.ProcessData(tick, ct); });

    //        if (result.Success)
    //        {
    //            Console.WriteLine("Subscribed to all ticker updates successfully.");
    //        }
    //        else
    //        {
    //            Console.WriteLine($"Failed to subscribe to ticker updates: {result.Error}");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex.ToString());
    //    }
    //}

    internal static void InvokeHandler<T>(T data, Action<T> handler)
    {
        if (Equals(data, default(T)!))
            return;

        handler?.Invoke(data!);
    }

    internal static T GetData<T>(DataEvent<JsonElement> tokenData)
    {
        JsonSvc jsonSvc = new JsonSvc();
        var desResult = jsonSvc.Deserialize<KucoinUpdateMessage<T>>(tokenData.Data);
        if (!desResult)
        {
            // _log.Write(LogLevel.Warning, "Failed to deserialize update: " + desResult.Error + ",
            // data: " + tokenData);
            return default!;
        }
        return desResult.Data.Data;
    }

    internal static string? TryGetSymbolFromTopic(DataEvent<JsonElement> data)
    {
        string? symbol = null;
        var topic = data.Data[0].ToString();
        if (topic != null && topic.Contains(':'))
            symbol = topic.Split(':').Last();
        return symbol;
    }
}