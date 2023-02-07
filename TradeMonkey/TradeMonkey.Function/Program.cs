var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()

    .ConfigureServices(services =>
    {
        // scans this assembly for classes decorated with the [RegisterService] attribute and adds
        // them to the ServicesCollection for dependency injection
        services.ScanCurrentAssembly();

        // avoid nested serialization
        services.Configure<JsonSerializerOptions>(options =>
        {
            options.ReferenceHandler = ReferenceHandler.IgnoreCycles; //doesn't add $id to json
            options.PropertyNameCaseInsensitive = true;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        // DbContext Pool
        //services.AddDbContextPool<FactorhawkContext>(options =>
        //  options.UseSqlServer(connectionString: Settings.FHConnectionString));

        services.AddHttpClient<TokenMetricsRepository>("ClientsMs", httpClient =>
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