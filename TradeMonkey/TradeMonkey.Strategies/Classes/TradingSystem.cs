using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TradeMonkey.Trader.Interfaces;

namespace TradeMonkey.Trader.Classes
{
    public class TradingSystem
    {
        private List<IStrategy> strategies;
        private IStrategy currentStrategy;
        private MarketMonitor marketMonitor;

        public TradingSystem()
        {
            strategies = new List<IStrategy> { new Strategy1(), new Strategy2(), new Strategy3() };
            marketMonitor = new MarketMonitor();
        }

        public void EvaluateMarket()
        {
            var conditions = marketMonitor.GetConditions();
            foreach (var strategy in strategies)
            {
                if (strategy.IsApplicable(conditions))
                {
                    currentStrategy = strategy;
                    break;
                }
            }
        }

        public void Trade()
        {
            currentStrategy?.Trade();
        }
    }
}