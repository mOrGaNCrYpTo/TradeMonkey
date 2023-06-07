using TradeMonkey.Services.Service;

namespace TradeMonkey.DataCollector.Strategies
{
    [RegisterService]
    public class BreakoutStrategy
    {
        private List<Kucoin.Net.Objects.Models.Spot.KucoinAccount> accounts;

        [InjectService]
        public KucoinOrderSvc KucoinOrderSvc { get; private set; }

        [InjectService]
        public KuCoinDbRepository kuCoinDbRepository { get; private set; }

        [InjectService]
        public KucoinAccountSvc KucoinAccountSvc { get; private set; }

        public int FastestMA { get; set; } = 50;
        public int FasterMA { get; set; } = 20;
        public int FastMA { get; set; } = 50;

        public int SlowMa { get; set; } = 200;

        public BreakoutStrategy()
        {
        }

        public async Task RunAnalysisAsync(IEnumerable<QuoteDto> quotes, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var fastPeriods = new Tuple<int, int, int>(FastMA, FasterMA, FastestMA);

            IEnumerable<AwesomeResult> awesomeResults =
                Indicator.GetAwesome(quotes: quotes,
                                              fastPeriods: 5,
                                              slowPeriods: 34);
        }
    }
}