namespace TradeMonkey.Trader.Rules
{
    public class RsiRule : ITradingRule
    {
        private readonly ILogger _logger;
        public int Period { get; set; }
        public TradingSignal CurrentSignal { get; set; }

        public RsiRule(int period, ILogger logger)
        {
            Period = period;
            CurrentSignal = TradingSignal.None;
            _logger = logger;
        }

        public async Task<TradingSignal> GetTradingSignalAsync(List<QuoteDto> quotes, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                var allRsis = await Task.Run(() => Indicator.GetRsi(quotes, Period));
                var rsi = allRsis.Last().Rsi!.Value;

                if (rsi > 70)
                {
                    CurrentSignal = TradingSignal.GoShort;
                }
                else if (rsi < 30)
                {
                    CurrentSignal = TradingSignal.GoLong;
                }
                else
                {
                    CurrentSignal = TradingSignal.None;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR in RsiRule.EvaluateAsync() - {ex.Message}");
                throw;
            }

            return CurrentSignal;
        }
    }
}