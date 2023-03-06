using TradeMonkey.Data.Entity;
using TradeMonkey.DataCollector.Utils;
using TradeMonkey.DataCollector.Value.Constant;

namespace TradeMonkey.DataCollector.Strategies
{
    public class BaseStrategy
    {
        public TradingInterval Timeframe { get; set; }
        public List<Indicator> Indicators { get; set; }
        public List<decimal> SupportResistanceLevels { get; set; }

        public bool CheckConditions(KucoinAllTick ticker)
        {
            foreach (Indicator indicator in Indicators)
            {
                if (!indicator.CheckCondition(ticker))
                    return false;
            }

            return true;
        }
    }
}