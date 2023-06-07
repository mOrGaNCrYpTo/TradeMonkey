using TradeMonkey.Data.Entity;
using TradeMonkey.Trader.Utils;
using TradeMonkey.Trader.Value.Constant;

namespace TradeMonkey.Trader.Strategies
{
    public class BaseStrategy
    {
        public TradingInterval Timeframe { get; set; }
        public List<Indicator> Indicators { get; set; }
        public List<decimal> SupportResistanceLevels { get; set; }

        public bool CheckConditions(Data.Entity.KucoinAllTick ticker)
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