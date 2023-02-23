#pragma warning disable IDE0160 // Convert to block scoped namespace

namespace TradeMonkey.Data.Context;
#pragma warning restore IDE0160 // Convert to block scoped namespace

public partial class TmDBContext : DbContext
{
    public virtual DbSet<IndicatorDatum> IndicatorDatum { get; set; }

    public virtual DbSet<KucoinAccounts> KucoinAccounts { get; set; }

    public virtual DbSet<KucoinIsolatedMarginAccounts> KucoinIsolatedMarginAccounts { get; set; }

    public virtual DbSet<KucoinIsolatedMarginAccountsInfo> KucoinIsolatedMarginAccountsInfo { get; set; }

    public virtual DbSet<KucoinKlines> KucoinKlines { get; set; }

    public virtual DbSet<KucoinTick> KucoinTicks { get; set; }

    public virtual DbSet<PricePredictionDatum> PricePredictionDatum { get; set; }

    public virtual DbSet<QuantmetricsT1Datum> QuantmetricsT1Datum { get; set; }

    public virtual DbSet<QuantmetricsT2Datum> QuantmetricsT2Datum { get; set; }

    public virtual DbSet<ResistanceSupportDatum> ResistanceSupportDatum { get; set; }

    public virtual DbSet<ScenarioAnalysisData> ScenarioAnalysisData { get; set; }

    public virtual DbSet<SentimentsDatum> SentimentsDatum { get; set; }

    public virtual DbSet<TokenMetricsTokens> TokenMetricsTokens { get; set; }

    public virtual DbSet<TraderGradesDatum> TraderGradesDatum { get; set; }

    public virtual DbSet<TradingIndicatorDatum> TradingIndicatorDatum { get; set; }

    public TmDBContext()
    {
    }

    public TmDBContext(DbContextOptions<TmDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.IndicatorDatumConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.KucoinAccountsConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.KucoinIsolatedMarginAccountsConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.KucoinIsolatedMarginAccountsInfoConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.KucoinKlinesConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.KucoinTicksConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.PricePredictionDatumConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.QuantmetricsT1DatumConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.QuantmetricsT2DatumConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ResistanceSupportDatumConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ScenarioAnalysisDataConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.SentimentsDatumConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TokenMetricsTokensConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TraderGradesDatumConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TradingIndicatorDatumConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}