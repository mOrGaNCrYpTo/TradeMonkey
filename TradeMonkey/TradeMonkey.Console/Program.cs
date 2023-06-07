namespace TradeMonkey.Console.App
{
    public static class Program
    {
        static async Task Main()
        {
            var config = InitializeConfig();
            var serviceProvider = ServiceHelpers.ConfigureServices(config);
            using var cts = new CancellationTokenSource();

            try
            {
                await RunAsync(serviceProvider, cts.Token);
            }
            catch (OperationCanceledException oce)
            {
                System.Console.WriteLine("Operation cancelled. " + oce.Message);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }

        private static DomainConfiguration InitializeConfig()
        {
            TradeMonkeyConfigurationBuilder _configurationBuilder = new("");
            return _configurationBuilder.DomainConfiguration;
        }

        static async Task RunAsync(ServiceProvider serviceProvider, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            await TimerHelpers.ExecuteKuCoinTimers(serviceProvider, ct);
            await TimerHelpers.ExecuteTokenMetricsTimers(serviceProvider, ct);

            // KEEP THE CONSOLE RUNNING BY WAITING FOR DATA INDEFINITELY
            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), ct);
            }
        }
    }
}