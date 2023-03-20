namespace TradeMonkey.DataCollector.Strategies
{
    public class BaseStrategy
    {
        public Symbol Symbol { get; set; }
        public ILogger Loggy { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal StopLossPrice { get; set; }
        public decimal TakeProfitPrice { get; set; }
        public decimal RiskPercent { get; set; } = 0.01m;
        public decimal TakeProfitPercent { get; set; } = 0.02m;
        public decimal RewardPercent { get; set; }
        public int OscillatorPeriod { get; set; }
        public int RsiPeriods { get; set; } = 14;
        public int SignalPeriods { get; set; }
        public int SmaFastPeriods { get; set; }
        public int SmaMedPeriods { get; set; }
        public int SmaSlowPeriods { get; set; }
        public int StopLossMultiplier { get; set; }

        // Add new properties for additional rules
        public int Ema50Periods { get; set; } = 50;

        public int Ema100Periods { get; set; } = 100;
        public int BollingerBandsPeriods { get; set; } = 20;
        public int BollingerBandsStdDev { get; set; } = 2;
        public int StochasticKLength { get; set; } = 7;
        public int StochasticKSmoothing { get; set; } = 3;
        public int StochasticDSmoothing { get; set; } = 3;
        public int Smma21Periods { get; set; } = 21;
        public int Smma50Periods { get; set; } = 50;
        public int Smma200Periods { get; set; } = 200;

        [InjectService]
        public TradingCalculators Calculators { get; set; }

        /// <summary>
        /// </summary>
        public BaseStrategy(TradingCalculators calculators, Symbol symbol, ILogger logger)
        {
            Calculators = calculators ?? throw new ArgumentNullException(nameof(calculators));
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
        public BaseStrategy(ILogger logger, int smaFastPeriods, int smaMedPeriods,
            int smaSlowPeriods, int rsiPeriods, int oscillatorPeriod, decimal riskPercent, decimal rewardPercent, int stopLossMultiplier)
        {
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