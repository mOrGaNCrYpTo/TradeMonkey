namespace TradeMonkey.Trader.Interfaces
{
    public enum TradingSignal
    {
        None,
        GoLong,
        GoShort,
        Hold
    }

    public interface ITradingRule
    {
        int Period { get; set; }
        TradingSignal CurrentSignal { get; set; }

        Task<TradingSignal> GetTradingSignalAsync(List<QuoteDto> quotes, CancellationToken ct = default);
    }
}