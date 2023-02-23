using Mapster;

namespace TradeMonkey.DataCollector.Helpers
{
    [RegisterService]
    public class KucoinTickerDataSvc
    {
        private readonly TmDBContext _dbContext;

        [InjectService]
        public ApiRepository ApiRepository { get; private set; }

        public KucoinTickerDataSvc(TmDBContext dBContext, ApiRepository apiRepository)
        {
            _dbContext = dBContext;
            ApiRepository = apiRepository;
        }

        public async Task ProcessTick(KucoinStreamTick streamtick, CancellationToken token)
        {
            var tick = streamtick.Adapt<Data.Entity.KucoinTick>();

            await _dbContext.KucoinTicks.AddAsync(tick);

            foreach (KucoinTick ticker in allTicks)
            {
                var tickerData = new KucoinTick
                {
                    Symbol = ticker.Symbol,
                    SymbolName = ticker.SymbolName,
                    ChangePrice = ticker.ChangePrice,
                    ChangePercentage = ticker.ChangePercentage,
                    HighPrice = ticker.HighPrice,
                    LastPrice = ticker.LastPrice,
                    LowPrice = ticker.LowPrice,
                    Volume = ticker.Volume,
                    QuoteVolume = ticker.QuoteVolume,
                    BestAskPrice = ticker.BestAskPrice,
                    BestBidPrice = ticker.BestBidPrice,
                    TakerCoefficient = ticker.TakerCoefficient,
                    MakerCoefficient = ticker.MakerCoefficient,
                    TakerFeeRate = ticker.TakerFeeRate,
                    AveragePrice = ticker.AveragePrice,
                    MakerFeeRate = ticker.MakerFeeRate,
                };

                _dbContext.KucoinTicks.Add(tickerData);
            }
            await _dbContext.SaveChangesAsync();
        }

        //public static async Task ProcessDataAsync(KucoinStreamTick tick, CancellationToken ct = default)
        //{
        //    // Add the current price to the high/low price lists, if it's not null
        //    if (tick.LastPrice.HasValue)
        //    {
        //        highPrices.Add(tick.LastPrice.Value);
        //        lowPrices.Add(tick.LastPrice.Value);

        // // Trim the lists to keep only the last n periods if (highPrices.Count > periodLength)
        // highPrices.RemoveAt(0); if (lowPrices.Count > periodLength) lowPrices.RemoveAt(0);

        // // Determine the highest high and lowest low over the last n periods var highestHigh =
        // highPrices.Max(); var lowestLow = lowPrices.Min();

        //        // Check for a breakout opportunity
        //        if (tick.LastPrice > highestHigh * breakoutThreshold)
        //        {
        //        }
        //        else if (tick.LastPrice < lowestLow * (2 - breakoutThreshold))
        //        {
        //            // Trigger a sell order ...
        //        }
        //    }
        //}
    }
}