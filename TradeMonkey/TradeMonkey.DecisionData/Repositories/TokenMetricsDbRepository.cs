namespace TradeMonkey.DecisionData.Repositories
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    [RegisterService]
    public sealed class TokenMetricsDbRepository
    {
        private readonly TmDBContext _dbContext;

        [ServiceConstructor]
        public TokenMetricsDbRepository(TmDBContext tmDBContext)
        {
            _dbContext = tmDBContext ?? throw new ArgumentNullException(nameof(tmDBContext));
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var x = _dbContext.Database.GetDbConnection().ConnectionString;
        }

        public async Task UpsertDataAsync<TEntity>(IEnumerable<TEntity> data, CancellationToken ct = default)
             where TEntity : class
        {
            ct.ThrowIfCancellationRequested();

            // Get the DbSet for the entity type
            DbSet<TEntity> dbSet = _dbContext.Set<TEntity>();

            await dbSet.BulkUpdateAsync(data, ct);

            // Save the changes to the database
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}