namespace TradeMonkey.TokenMetrics.Domain.Value.Constants
{
    /// <summary>
    /// Provides easy access to Environment Variables. Local instances are in local.settings.json
    /// </summary>
    public static class Settings
    {
        public static string AzureAppConfiguration { get; set; } = Environment.GetEnvironmentVariable("AZURE_APP_CONFIG");
        public static string FHConnectionString { get; set; } = Environment.GetEnvironmentVariable("FACTORHAWK_DB");
        public static string KuCoinBaseApi { get; set; } = Environment.GetEnvironmentVariable("KUCOIN_BASE_API");
        public static bool MetricsLoggingEnabled { get; set; } = Boolean.Parse(Environment.GetEnvironmentVariable("METRICS_LOGGING_ENABLED"));
        public static string TokenMetricsBaseUrl { get; set; } = Environment.GetEnvironmentVariable("TOKEN_METRICS_BASE_URL");
    }
}