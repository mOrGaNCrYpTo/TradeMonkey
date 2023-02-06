// Set up the environment and variables
using System;
using System.Security.Cryptography;

using QuantConnect;
using QuantConnect.Algorithm;
using QuantConnect.Data;
using QuantConnect.Data.Market;

//Instantiate the algorithm
public class CryptoScalpingAlgorithm : QCAlgorithm
{
    //Set the starting capital
    public override void Initialize()
    {
        SetStartDate(2017, 01, 01);
        SetEndDate(2018, 01, 01);
        SetCash(500);
    }

    //Create a 5m bar
    public void OnData(TradeBars data)
    {
        DateTime time = data.Time;
        int barCount = 20;
        if (time.Minute % 5 == 0)
        {
            // Calculate the moving average of the last 20 bars
            decimal movingAverage = 0;
            for (int i = 0; i < barCount; i++)
            {
                movingAverage += data[Symbols.BTCUSD].Close;
            }
            movingAverage = movingAverage / barCount;

            // Trade long
            if (data[Symbols.BTCUSD].Close > movingAverage)
            {
                SetHoldings(Symbols.BTCUSD, 0.99);
                // Calculate the commission
                decimal commission = 0.01m * 0.01m * data[Symbols.BTCUSD].Close;
                // Adjust the holdings accordingly
                AdjustCash(-commission);
            }

            // Trade short
            if (data[Symbols.BTCUSD].Close < movingAverage)
            {
                SetHoldings(Symbols.BTCUSD, -0.99);
                // Calculate the commission
                decimal commission = 0.01m * 0.01m * data[Symbols.BTCUSD].Close;
                // Adjust the holdings accordingly
                AdjustCash(-commission);
            }
        }
    }
}