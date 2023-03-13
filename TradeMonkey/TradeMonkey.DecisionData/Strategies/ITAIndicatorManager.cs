using Skender.Stock.Indicators;

namespace TradeMonkey.Trader.Strategies
{
    public interface ITAIndicatorManager
    {
        decimal GetAtr(IEnumerable<IQuote> quotes, int period);
        decimal GetLinearRegression(IEnumerable<IQuote> quotes, int periods);
        decimal GetResistanceLevel(IEnumerable<IQuote> quotes);
        IEnumerable<RollingPivotsResult> GetRollingPivots(IEnumerable<IQuote> quotes, int windowPeriods, int offsetPeriods, PivotPointType pointType = PivotPointType.Standard);
        decimal GetRsi(IEnumerable<IQuote> quotes, int periods);
        Task<decimal> GetSlopeAsync(IEnumerable<IQuote> quotes, int periods);
        decimal GetSma(IEnumerable<IQuote> quotes, int period);
        decimal GetStochasticRSI(IEnumerable<IQuote> quotes, int rsiPeriods, int stochPeriods, int signalPeriods, int smoothPeriods);
        decimal GetSupportLevel(IEnumerable<IQuote> quotes);
    }
}