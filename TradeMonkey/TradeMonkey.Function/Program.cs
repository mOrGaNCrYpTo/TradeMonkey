var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()

    .ConfigureServices(services =>
    {
        services.AddHttpClient();
    })

    .Build();

host.Run();