namespace TradeMonkey.Data.Context
{
    public partial class TmDBContext : DbContext
    {
        public virtual DbSet<CorrelationDatum> CorrelationDatum { get; set; }

        public virtual DbSet<IndicatorDatum> IndicatorDatum { get; set; }

        public virtual DbSet<IndiciesDatum> IndiciesDatum { get; set; }

        public virtual DbSet<KucoinTickerHistories> KucoinTickerHistories { get; set; }

        public virtual DbSet<PricePredictionDatum> PricePredictionDatum { get; set; }

        public virtual DbSet<QuantmetricsT1Datum> QuantmetricsT1Datum { get; set; }

        public virtual DbSet<QuantmetricsT2Datum> QuantmetricsT2Datum { get; set; }

        public virtual DbSet<ResistanceSupportDatum> ResistanceSupportDatum { get; set; }

        public virtual DbSet<ScenarioAnalysisDatum> ScenarioAnalysisDatum { get; set; }

        public virtual DbSet<SentimentsDatum> SentimentsDatum { get; set; }

        public virtual DbSet<Tokens> Tokens { get; set; }

        public virtual DbSet<TraderGradesDatum> TraderGradesDatum { get; set; }

        public virtual DbSet<TradingIndicatorDatum> TradingIndicatorDatum { get; set; }

        public TmDBContext()
        {
        }

        public TmDBContext(DbContextOptions<TmDBContext> options)
            : base(options)
        {
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                    => optionsBuilder.UseSqlServer("Data Source=HP\\MFSQL;Initial Catalog=TradeMonkey;Integrated Security=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.CorrelationDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.IndiciesDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.KucoinTickerHistoriesConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PricePredictionDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.QuantmetricsT1DatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.QuantmetricsT2DatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ResistanceSupportDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ScenarioAnalysisDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SentimentsDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TokensConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TraderGradesDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TradingIndicatorDatumConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }
    }
}