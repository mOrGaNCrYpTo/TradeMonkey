namespace TradeMonkey.DataCollector.Repositories
{
    public sealed class KuCoinDbRepository
    {
        private readonly TmDBContext _dbContext;

        public KuCoinDbRepository(TmDBContext tmDBContext)
        {
            _dbContext = tmDBContext ?? throw new ArgumentNullException(nameof(tmDBContext));
        }

        public async Task<IEnumerable<Kucoin_AllTick>> GetTickerHistoriesAsync(List<string> symbols, int periodLength,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            return await
                _dbContext.Kucoin_AllTicks
                    .OrderByDescending(kt => kt.timestamp)
                    .Where(t => symbols.Contains(t.symbol))
                    .Take(periodLength)
                    .ToListAsync(ct);
        }

        public async Task InsertTickerData(IEnumerable<Kucoin_AllTick> data, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            _dbContext.Set<Kucoin_AllTick>().AddRange(data);
            _dbContext.SaveChanges();
        }

        public async Task UpsertOrderDataAsync(IEnumerable<Kucoin_Order> data, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            _dbContext.Set<Kucoin_Order>().UpdateRange(data);
            _dbContext.SaveChanges();
        }
    }
}