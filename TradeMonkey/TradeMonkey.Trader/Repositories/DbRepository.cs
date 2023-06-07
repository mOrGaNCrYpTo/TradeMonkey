using Newtonsoft.Json.Converters;

using TradeMonkey.Data.Entity;

namespace TradeMonkey.Trader.Repositories
{
    public sealed class DbRepository
    {
        public TmDBContext DbContext { get; private set; }

        public DbRepository(TmDBContext tmDBContext)
        {
            DbContext = tmDBContext ?? throw new ArgumentNullException(nameof(tmDBContext));
        }

        public async Task<IEnumerable<Data.Entity.KucoinAllTick>> GetTickerDataAsync(string symbol, int periodLength,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            return
                await DbContext.KucoinAllTicks
                    .OrderByDescending(kt => kt.Timestamp)
                    .Where(t => t.Symbol == symbol)
                    .Take(periodLength)
                    .ToListAsync(ct);
        }
    }
}