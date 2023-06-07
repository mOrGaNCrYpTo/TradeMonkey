using TradeMonkey.DataCollector.Strategies;
using TradeMonkey.Services.Service;
using Microsoft.Extensions.Logging;
using TradeMonkey.Trader.Classes;

public class MovingAverageCrossoverStrategy : BaseStrategy
{
    public int ShortTermPeriods { get; set; }
    public int LongTermPeriods { get; set; }

    public MovingAverageCrossoverStrategy(TradingCalculators calculators, Symbol symbol, ILogger logger, int shortTermPeriods, int longTermPeriods)
        : base(calculators, symbol, logger)
    {
        ShortTermPeriods = shortTermPeriods;
        LongTermPeriods = longTermPeriods;
    }

    public TradeAction DetermineAction(TradeContext context)
    {
        var shortTermAverage = Calculators.CalculateMovingAverage(context.HistoricalPrices, ShortTermPeriods);
        var longTermAverage = Calculators.CalculateMovingAverage(context.HistoricalPrices, LongTermPeriods);

        if (shortTermAverage > longTermAverage)
        {
            return TradeAction.Buy;
        }
        else if (shortTermAverage < longTermAverage)
        {
            return TradeAction.Sell;
        }

        return TradeAction.Hold;
    }
}