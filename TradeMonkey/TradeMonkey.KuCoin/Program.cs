using Kucoin.Net;
using Kucoin.Net.Objects;
using Kucoin.Net.Enums;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()

    .ConfigureServices(services =>
    {
        // scans this assembly for classes decorated with the [RegisterService] attribute and adds
        // them to the ServicesCollection for dependency injection
        services.ScanCurrentAssembly();

        var credentials = new KucoinApiCredentials("63f251c80c3f5700017762ff", "c7b1a1f8-eac8-40bf-9461-56cc24deb1a4", "89t@UzifA$Hb6p5");

        var socketOptions = new KucoinSocketApiClientOptions
        {
            ApiCredentials = credentials,
            BaseAddress = KucoinClientOptions.Default.SpotApiOptions.BaseAddress
        };

        services.AddKucoin();

        // avoid nested serialization
        services.Configure<JsonSerializerOptions>(options =>
        {
            options.ReferenceHandler = ReferenceHandler.IgnoreCycles; //doesn't add $id to json
            options.PropertyNameCaseInsensitive = true;
            //options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        // DbContext Pool
        //services.AddDbContextPool<FactorhawkContext>(options =>
        //  options.UseSqlServer(connectionString: Settings.FHConnectionString));

        services.AddHttpClient<ApiRepository>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(uriString: Settings.TokenMetricsBaseUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Trade Monkey");
            httpClient.DefaultRequestHeaders.Add("api_key", "tm-682d1310-5f3e-4333-9dcb-2f6a31eec886");
        })
            .SetHandlerLifetime(TimeSpan.FromMinutes(10));
    })

    .Build();

host.Run();