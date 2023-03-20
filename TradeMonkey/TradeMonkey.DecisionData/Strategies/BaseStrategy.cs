using CryptoExchange.Net.CommonObjects;

using TradeMonkey.Trader.Helpers;

namespace TradeMonkey.DataCollector.Strategies
{
    public class BaseStrategy
    {
        public Symbol Symbol { get; set; }
        public ILogger Loggy { get; set; }
        public TradingCalculators Calculators { get; set; }

        public decimal EntryPrice { get; set; }
        public decimal StopLossPrice { get; set; }
        public decimal TakeProfitPrice { get; set; }
        public decimal RewardPercent { get; set; }
        public decimal RiskPercent { get; set; }
        public int OscillatorPeriod { get; set; }
        public int RsiPeriods { get; set; }
        public int SignalPeriods { get; set; }
        public int SmaFastPeriods { get; set; }
        public int SmaMedPeriods { get; set; }
        public int SmaSlowPeriods { get; set; }
        public int StopLossMultiplier { get; set; }

        /// <summary>
        /// </summary>
        public BaseStrategy(TradingCalculators tradingCalculators, Symbol symbol, ILogger logger)
        {
            Calculators = tradingCalculators ?? throw new ArgumentNullException(nameof(tradingCalculators));
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            Loggy = logger ?? throw new ArgumentNullException();
        }

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
        public BaseStrategy(TradingCalculators tradingCalculators, ILogger logger, int smaFastPeriods, int smaMedPeriods,
            int smaSlowPeriods, int rsiPeriods, int oscillatorPeriod, decimal riskPercent, decimal rewardPercent, int stopLossMultiplier)
        {
            Calculators = tradingCalculators ?? throw new ArgumentNullException(nameof(tradingCalculators));
            Loggy = logger ?? throw new ArgumentNullException(nameof(logger));

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