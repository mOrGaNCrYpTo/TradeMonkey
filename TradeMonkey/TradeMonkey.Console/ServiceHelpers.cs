namespace TradeMonkey.Console.Helpers
{
    internal static class ServiceHelpers
    {
        public static ServiceProvider ConfigureServices(DomainConfiguration config)
        {
            var services = new ServiceCollection();

            services.Configure<JsonSerializerOptions>(options =>
            {
                options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.PropertyNameCaseInsensitive = true;
            });

            services.AddDbContext<TmDBContext>(options =>
                options.UseSqlServer(config.DatabaseSettings.ConnectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );

            services.AddScoped<KuCoinDbRepository>();
            services.AddScoped<KucoinClient>();
            services.AddScoped<KucoinAccountSvc>();
            services.AddScoped<KucoinSocketClient>();
            services.AddScoped<KucoinTickerSvc>();
            services.AddScoped<TokenMetricsSvc>();

            string apiKey = config.KucoinApi.ApiKey;
            string secretKey = config.KucoinApi.ApiSecret;
            string apiPass = config.KucoinApi.ApiPassphrase;

            var apiCredentials = new KucoinApiCredentials(apiKey, secretKey, apiPass);

            var kucoinClientOptions = new KucoinClientOptions
            {
                ApiCredentials = apiCredentials,
                LogLevel = LogLevel.Debug
            };

            services.AddKucoin((restClientOptions, socketClientOptions) =>
            {
                restClientOptions.ApiCredentials = kucoinClientOptions.ApiCredentials;
                restClientOptions.LogLevel = LogLevel.Trace;
                socketClientOptions.ApiCredentials = kucoinClientOptions.ApiCredentials;
                socketClientOptions.SpotStreamsOptions.AutoReconnect = true;
                socketClientOptions.SpotStreamsOptions.ReconnectInterval = TimeSpan.FromSeconds(10);
                socketClientOptions.SpotStreamsOptions.SocketResponseTimeout = TimeSpan.FromSeconds(10);
            });

            services.AddSingleton(provider =>
            {
                var uriBuilder = new UriBuilder
                {
                    Scheme = "https",
                    Host = config.TokenMetricsApi.ApiBaseUrl
                };
                return uriBuilder;
            });

            services.AddHttpClient<TokenMetricsApiRepository>(httpClient =>
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Trade.Monkey");
                httpClient.DefaultRequestHeaders.Add(
                    config.TokenMetricsApi.ApiKeyName,
                    config.TokenMetricsApi.ApiKeyVal);
                httpClient.Timeout = TimeSpan.FromMinutes(10);
            })
                .SetHandlerLifetime(TimeSpan.FromMinutes(10));

            services.ScanCurrentAssembly(ServiceDescriptorMergeStrategy.TryAdd);

            return services.BuildServiceProvider();
        }
    }
}