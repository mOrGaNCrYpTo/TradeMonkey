using TradeMonkey.Data.Entity;
using TradeMonkey.DataCollector.Extensions;

namespace TradeMonkey.DataCollector.Utils
{
    public abstract class Indicator
    {
        public abstract bool CheckCondition(KucoinAllTick ticker);
    }

    public class MovingAverage : Indicator
    {
        private int _period1;
        private int _period2;

        public MovingAverage(int period1, int period2)
        {
            _period1 = period1;
            _period2 = period2;
        }

        public override bool CheckCondition(KucoinAllTick ticker)
        {
            //decimal ma1 = ticker.CalculateMovingAverage(_period1);
            // decimal ma2 = ticker.CalculateMovingAverage(_period2);

            // return ma1 > ma2;
            return false;
        }
    }

    public class RelativeStrengthIndex : Indicator
    {
        private int _period;

        public RelativeStrengthIndex(int period)
        {
            _period = period;
        }

        public override bool CheckCondition(KucoinAllTick ticker)
        {
            //decimal rsi = ticker.CalculateRelativeStrengthIndex(_period);
            //return rsi > 70;
            return false;
        }
    }

    public class StochasticOscillator : Indicator
    {
        private int _period;

        public StochasticOscillator(int period)
        {
            _period = period;
        }

        public override bool CheckCondition(KucoinAllTick ticker)
        {
            decimal stochastic = 0m; // ticker.CalculateStochasticOscillator(_period);

            return stochastic > 70;
        }
    }
}