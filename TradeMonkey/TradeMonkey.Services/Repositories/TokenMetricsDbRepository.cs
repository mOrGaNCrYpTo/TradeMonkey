namespace TradeMonkey.Services.Repositories
{
    [RegisterService]
    public sealed class TokenMetricsDbRepository
    {
        private readonly TmDBContext _dbContext;

        [ServiceConstructor]
        public TokenMetricsDbRepository(TmDBContext tmDBContext)
        {
            _dbContext = tmDBContext ?? throw new ArgumentNullException(nameof(tmDBContext));
            //_dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task InsertAsync(IEnumerable<dynamic> data, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            // Get the DbSet for the entity type
            await _dbContext.AddRangeAsync(data, ct);

            // Save the changes to the database
            _ = await _dbContext.SaveChangesAsync(ct);
        }

        public async Task UpsertAsync(IEnumerable<dynamic> data, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            // Get the DbSet for the entity type
            await Task.Run(() => _dbContext.UpdateRange(data), ct);

            // Save the changes to the database
            _ = await _dbContext.SaveChangesAsync(ct);
        }

        public async Task BulkUpdateAsync<TEntity>(IEnumerable<TEntity> data, CancellationToken ct = default)
            where TEntity : class
        {
            ct.ThrowIfCancellationRequested();

            // Get the DbSet for the entity type
            DbSet<TEntity> dbSet = _dbContext.Set<TEntity>();

            await dbSet.BulkUpdateAsync(data, ct);

            // Save the changes to the database
            _ = await _dbContext.SaveChangesAsync(ct);
        }

        public async Task BulkInsertDataAsync<TEntity>(IEnumerable<TEntity> data, CancellationToken ct = default)
             where TEntity : class
        {
            ct.ThrowIfCancellationRequested();

            // Get the DbSet for the entity type
            DbSet<TEntity> dbSet = _dbContext.Set<TEntity>();

            await dbSet.BulkInsertAsync(data, ct);

            // Save the changes to the database
            _ = await _dbContext.SaveChangesAsync(ct);
        }

        public async Task BulkDelete<TEntity>(IEnumerable<TEntity> data, CancellationToken ct = default)
            where TEntity : class
        {
            ct.ThrowIfCancellationRequested();

            // Get the DbSet for the entity type
            DbSet<TEntity> dbSet = _dbContext.Set<TEntity>();

            await dbSet.BulkDeleteAsync(data, ct);

            // Save the changes to the database
            _ = await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<List<int>> GetTokenIdsByName(IEnumerable<string> symbols, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            // Save the changes to the database
            return await _dbContext.TokenMetricsTokens
                            .Where(x => symbols.Contains(x.Symbol))
                            .Select(x => x.TokenId)
                            .ToListAsync(ct);
        }
    }
}