namespace TradeMonkey.Function.Trigger.Timer
{
    public class FiveMinutes
    {
        private readonly ILogger _logger;

        public FiveMinutes(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FiveMinutes>();
        }

        [Function(nameof(FiveMinutes))]
        public void Run([TimerTrigger("0 */5 * * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }

    public class MyInfo
    {
        public bool IsPastDue { get; set; }
        public MyScheduleStatus ScheduleStatus { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime LastUpdated { get; set; }
        public DateTime Next { get; set; }
    }
}