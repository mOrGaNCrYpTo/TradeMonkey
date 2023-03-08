﻿namespace TradeMonkey.Data.Context
{
    public partial class TmDBContext : DbContext
    {
        public virtual DbSet<CorrelationDatum> CorrelationDatums { get; set; }

        public virtual DbSet<IndicatorDatum> IndicatorDatums { get; set; }

        public virtual DbSet<IndiciesDatum> IndiciesDatums { get; set; }

        public virtual DbSet<Kucoin24hourStats> Kucoin24hourStats { get; set; }

        public virtual DbSet<KucoinAccount> KucoinAccounts { get; set; }

        public virtual DbSet<KucoinAllTick> KucoinAllTicks { get; set; }

        public virtual DbSet<KucoinTokenMetricsSymbol> KucoinTokenMetricsSymbols { get; set; }

        public virtual DbSet<PricePredictionDatum> PricePredictionDatums { get; set; }

        public virtual DbSet<QuantmetricsT1Datums> QuantmetricsT1Datums { get; set; }

        public virtual DbSet<QuantmetricsT2Datums> QuantmetricsT2Datums { get; set; }

        public virtual DbSet<ResistanceSupportDatum> ResistanceSupportDatums { get; set; }

        public virtual DbSet<ScenarioAnalysisDatum> ScenarioAnalysisDatums { get; set; }

        public virtual DbSet<SentimentsDatum> SentimentsDatums { get; set; }

        public virtual DbSet<TokenMetricsPrice> TokenMetricsPrices { get; set; }

        public virtual DbSet<TokenMetricsToken> TokenMetricsTokens { get; set; }

        public virtual DbSet<TraderGradesDatum> TraderGradesDatums { get; set; }

        public virtual DbSet<TradingIndicatorDatum> TradingIndicatorDatums { get; set; }

        public TmDBContext()
        {
        }

        public TmDBContext(DbContextOptions<TmDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.CorrelationDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.IndicatorDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.IndiciesDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.Kucoin24hourStatsConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.KucoinAccountConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.KucoinAllTickConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.KucoinTokenMetricsSymbolConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PricePredictionDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.QuantmetricsT1DatumsConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.QuantmetricsT2DatumsConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ResistanceSupportDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ScenarioAnalysisDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SentimentsDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TokenMetricsPriceConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TokenMetricsTokenConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TraderGradesDatumConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TradingIndicatorDatumConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}