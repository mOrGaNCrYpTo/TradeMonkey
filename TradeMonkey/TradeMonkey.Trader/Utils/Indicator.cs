namespace TradeMonkey.Trader.Utils
{
    using TradeMonkey.Trader.Extensions;

    using KucoinAllTick = TradeMonkey.Data.Entity.KucoinAllTick;

    public abstract class Indicator
    {
        public abstract bool CheckCondition(KucoinAllTick ticker, List<decimal> prices, int period);
    }

    public class MovingAverage : Indicator
    {
        public MovingAverage(int period1, int period2)
        {
            period1 = period1;
            period2 = period2;
        }

        // todo
        public override bool CheckCondition(KucoinAllTick ticker, List<decimal> prices, int period)
        {
            return true;
        }
    }

    public class RelativeStrengthIndex : Indicator
    {
        private int _period;

        public RelativeStrengthIndex(int period)
        {
            _period = period;
        }

        // todo
        public override bool CheckCondition(KucoinAllTick ticker, List<decimal> prices, int period)
        {
            decimal rsi = ticker.CalculateRelativeStrengthIndex(period, prices);
            return rsi > 70;
        }
    }

    public class StochasticOscillator : Indicator
    {
        private int _period;

        public StochasticOscillator(int period)
        {
            _period = period;
        }

        public override bool CheckCondition(KucoinAllTick ticker, List<decimal> prices, int period)
        {
            decimal stochastic = ticker.CalculateStochasticOscillator(prices, period);
            return stochastic > 70;
        }
    }
}