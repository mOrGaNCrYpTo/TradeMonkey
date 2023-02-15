namespace TradeMonkey.KuCoin.Domain.Value.Constants
{
    public static class FunctionEvents
    {
        public static string OperationCanceled { get; set; } = "OPERAION_CANCELED";
        public static string TokenMetricsInvalidRequest { get; set; } = "TOKEN_METRICS_INAVALID_REQUEST";
        public static string TokenMetricsRequestCompleted { get; set; } = "TOKEN_METRICS_REQUEST_COMPLETED";
        public static string TokenMetricsRequestStarted { get; set; } = "TOKEN_METRICS_REQUEST_STARTED";
        public static string UnknownExceptionOccured { get; set; } = "UNKNOWN_EXCEPTION";
    }
}