namespace TradeMonkey.Core.Value.Constant
{
    public static class Settings
    {
        public static string KucoinProdApiUrl { get; set; } = "https://api.kucoin.com/api/v1/";
        public static string KucoinSandboxApiUrl { get; set; } = "https://api.kucoin.com/api/v1/";

        public static string KucoinApiKeyName { get; set; } = "KC-API-KEY";
        public static string KucoinApiSecretName { get; set; } = "KC-API-SIGN";
        public static string KucoinApiPassName { get; set; } = "KC-API-PASSPHRASE";

        public static string KucoinApiKeyVal { get; set; } = "63f3a3999ba1f40001e8c1a0";
        public static string KucoinApiSecretVal { get; set; } = "3abfb8ef-498e-43a7-8d8c-b500fdea0991";
        public static string KucoinApiPassVal { get; set; } = "89t@UzifA$Hb6p5";

        public static string TokenMetricsApiKeyName { get; set; } = "api_key";
        public static string TokenMetricsApiKeyVal { get; set; } = "tm-957bdb5c-d4e4-433d-91d5-0bf60e8ae129";
        public static string TokenMetricsApiBaseUrl { get; set; } = "api.tokenmetrics.com";

        public static string TradeMonkeyDb { get; set; } =
            @"Data Source=OTRROS-2WV24B3\CMORGAN;Database=TradeMonkey;Integrated Security=True;Encrypt=false;TrustServerCertificate=True";

        //"Data Source=HP\\MFSQL;Database=TradeMonkey;Integrated Security=True;Encrypt=false;TrustServerCertificate=True";
    }
}