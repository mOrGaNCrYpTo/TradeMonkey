using Newtonsoft.Json.Converters;

using TradeMonkey.Data.Entity;

namespace TradeMonkey.DataCollector.Repositories
{
    public sealed class DbRepository
    {
        public TmDBContext DbContext { get; private set; }

        public DbRepository(TmDBContext tmDBContext)
        {
            DbContext = tmDBContext ?? throw new ArgumentNullException(nameof(tmDBContext));
        }

        public async Task<IEnumerable<Kucoin_AllTick>> GetTickerDataAsync(string symbol, int periodLength,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            return
                await DbContext.Kucoin_AllTicks
                    .OrderByDescending(kt => kt.timestamp)
                    .Where(t => t.symbol == symbol)
                    .Take(periodLength)
                    .ToListAsync(ct);
        }
    }
}