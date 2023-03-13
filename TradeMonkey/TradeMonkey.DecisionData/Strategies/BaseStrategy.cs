using CryptoExchange.Net.CommonObjects;

namespace TradeMonkey.DataCollector.Strategies
{
    public class BaseStrategy
    {
        public Symbol Symbol { get; set; }
        public ILogger Logger { get; set; }
        public TradingCalculators TradingCalculators { get; set; }

        public int SmaFastPeriods { get; set; }
        public int SmaMedPeriods { get; set; }
        public int SmaSlowPeriods { get; set; }
        public int RsiPeriods { get; set; }
        public int OscillatorPeriod { get; set; }
        public decimal RiskPercent { get; set; }
        public decimal RewardPercent { get; set; }
        public int StopLossMultiplier { get; set; }

        /// <summary>
        /// </summary>
        public BaseStrategy() { }

        /// <summary>
        /// </summary>
        /// <param name="smaFastPeriods">     </param>
        /// <param name="smaMedPeriods">      </param>
        /// <param name="smaSlowPeriods">     </param>
        /// <param name="rsiPeriods">         </param>
        /// <param name="oscillatorPeriod">   </param>
        /// <param name="riskPercent">        </param>
        /// <param name="rewardPercent">      </param>
        /// <param name="stopLossMultiplier"> </param>
        public BaseStrategy(int smaFastPeriods, int smaMedPeriods, int smaSlowPeriods, int rsiPeriods,
               int oscillatorPeriod, decimal riskPercent, decimal rewardPercent, int stopLossMultiplier)
        {
            SmaFastPeriods = smaFastPeriods;
            SmaMedPeriods = smaMedPeriods;
            SmaSlowPeriods = smaSlowPeriods;
            RsiPeriods = rsiPeriods;
            OscillatorPeriod = oscillatorPeriod;
            RiskPercent = riskPercent;
            RewardPercent = rewardPercent;
            StopLossMultiplier = stopLossMultiplier;
        }
    }
}