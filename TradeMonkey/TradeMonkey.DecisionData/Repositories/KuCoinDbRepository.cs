namespace TradeMonkey.DataCollector.Repositories
{
    public sealed class KuCoinDbRepository
    {
        public TmDBContext DbContext { get; private set; }

        public KuCoinDbRepository(TmDBContext tmDBContext)
        {
            DbContext = tmDBContext ?? throw new ArgumentNullException(nameof(tmDBContext));
        }

        public async Task<IEnumerable<Kucoin_AllTick>> GetTickerHistoriesAsync(List<string> symbols, int periodLength,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            return await
                DbContext.Kucoin_AllTicks
                    .OrderByDescending(kt => kt.timestamp)
                    .Where(t => symbols.Contains(t.symbol))
                    .Take(periodLength)
                    .ToListAsync(ct);
        }

        public async Task InsertTickerData(IEnumerable<Kucoin_AllTick> data, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            DbContext.Set<Kucoin_AllTick>().AddRange(data);
            DbContext.SaveChanges();
        }
    }
}