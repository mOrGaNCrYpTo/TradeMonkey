using TradeMonkey.Core.Value.Aggregate;
using TradeMonkey.Trader.Value.Constant;

namespace TradeMonkey.Trader.Strategies
{
    public interface IStrategy<TQuote>
    {
        Task<TradingSignal> GetTradingSignalAsync(IEnumerable<QuoteDto> history, CancellationToken cancellationToken = default);

        Task<(decimal, decimal)> GetStopLossTakeProfitPricesAsync(IEnumerable<TQuote> history, CancellationToken cancellationToken = default);

        Task<decimal> GetExitPriceAsync(IEnumerable<TQuote> history, CancellationToken cancellationToken = default);

        Task<decimal> GetPositionSizeAsync(IEnumerable<TQuote> history, CancellationToken cancellationToken = default);

        Task<TimeSpan> GetDurationAsync(IEnumerable<TQuote> history, CancellationToken cancellationToken = default);
    }
}