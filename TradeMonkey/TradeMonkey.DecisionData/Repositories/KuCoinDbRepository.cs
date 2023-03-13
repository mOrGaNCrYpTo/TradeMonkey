using TradeMonkey.Trader.Value.Aggregate;

namespace TradeMonkey.Trader.Repositories
{
    [RegisterService]
    public sealed class KuCoinDbRepository
    {
        private readonly TmDBContext _dbContext;

        public KuCoinDbRepository(TmDBContext tmDBContext)
        {
            _dbContext = tmDBContext ?? throw new ArgumentNullException(nameof(tmDBContext));
        }

        public async Task<KucoinTopTokens> GetTopTokensAsync(int thresholdVolume = 1000000,
            double thresholdChange = 0.05, int numberOfTokens = 10, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            KucoinTopTokens topTokens = new();

            // DAY: Select tokens with high trading volume and significant price movements in the
            // past 24 hours
            var tokens24h = from t in _dbContext.KucoinAllTicks
                            where t.Timestamp > DateTime.UtcNow.AddHours(-24)
                            group t by t.Symbol into g
                            select new TopTokenInfo
                            {
                                Symbol = g.Key,
                                Volume = g.Sum(x => x.VolValue),
                                Change = (g.Max(x => x.High) - g.Min(x => x.Low)) / g.Min(x => x.Low)
                            };

            topTokens.HighVolumeDaily
                        = await tokens24h.Where(t => t.Volume > thresholdVolume)
                                         .OrderByDescending(t => t.Volume)
                                         .Take(numberOfTokens)
                                         .ToListAsync(cancellationToken: ct);

            topTokens.SignificantChangeDaily
                        = await tokens24h.Where(t => t.Change > thresholdChange)
                                         .OrderByDescending(t => t.Change)
                                         .Take(numberOfTokens)
                                         .ToListAsync(cancellationToken: ct);

            // WEEK: Select tokens with high trading volume and significant price movements in the
            // past week
            var tokensWeek = from t in _dbContext.KucoinAllTicks
                             where t.Timestamp > DateTime.UtcNow.AddDays(-7)
                             group t by t.Symbol into g
                             select new
                             {
                                 Symbol = g.Key,
                                 Volume = g.Sum(x => x.VolValue),
                                 Change = (g.Max(x => x.High) - g.Min(x => x.Low)) / g.Min(x => x.Low)
                             };

            topTokens.HighVolumeWeely
                 = await tokensWeek
                     .Where(t => t.Volume > thresholdVolume)
                     .OrderByDescending(t => t.Volume)
                     .Take(numberOfTokens)
                     .Select(t => new TopTokenInfo { Symbol = t.Symbol, Volume = t.Volume })
                     .ToListAsync(cancellationToken: ct);

            topTokens.SignificantChangeWeekly
                 = await tokensWeek
                     .Where(t => t.Volume > thresholdVolume)
                     .OrderByDescending(t => t.Volume)
                     .Take(numberOfTokens)
                     .Select(t => new TopTokenInfo { Symbol = t.Symbol, Change = t.Change })
                     .ToListAsync(cancellationToken: ct);

            return topTokens;
        }

        public async Task InsertManyAsync<TEntity>(IEnumerable<TEntity> data, CancellationToken ct) where TEntity : class
        {
            ct.ThrowIfCancellationRequested();

            await _dbContext.Set<TEntity>().BulkInsertAsync(data, ct);
            await _dbContext.SaveChangesAsync();
        }

        public async Task InsertOneAsync<TEntity>(TEntity data, CancellationToken ct) where TEntity : class
        {
            ct.ThrowIfCancellationRequested();

            await _dbContext.Set<TEntity>().SingleInsertAsync(data, ct);
            await _dbContext.SaveChangesAsync();
        }

        public void InsertOne<TEntity>(TEntity data) where TEntity : class
        {
            _dbContext.Set<TEntity>().SingleInsert(data);
            _dbContext.SaveChanges();
        }
    }
}