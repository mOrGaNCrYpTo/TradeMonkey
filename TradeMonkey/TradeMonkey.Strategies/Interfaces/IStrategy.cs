namespace TradeMonkey.Trader.Interfaces
{
    public interface IStrategy<TQuote>
    {
        Task<TradingSignal> ExecuteStrategyAsync(List<QuoteDto> quotes, CancellationToken ct);

        bool IsApplicable(MarketConditions conditions);

        void Trade();
    }
}