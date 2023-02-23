namespace TradeMonkey.Data.Context.Configurations
{
    public partial class KucoinTickConfiguration : IEntityTypeConfiguration<KucoinTick>
    {
        public void Configure(EntityTypeBuilder<KucoinTick> entity)
        {
            entity.ToTable("KucoinTicks");

            entity.Property(e => e.Symbol)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.LastPrice)
                .HasColumnType("decimal(18,9)");

            entity.Property(e => e.LastQuantity)
                .HasColumnType("decimal(18,9)");

            entity.Property(e => e.BestAskPrice)
                .HasColumnType("decimal(18,9)");

            entity.Property(e => e.BestAskQuantity)
                .HasColumnType("decimal(18,9)");

            entity.Property(e => e.BestBidPrice)
                .HasColumnType("decimal(18,9)");

            entity.Property(e => e.BestBidQuantity)
                .HasColumnType("decimal(18,9)");

            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime2")
                .IsRequired();

            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Symbol);

            entity.HasIndex(e => e.Timestamp);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<KucoinTick> entity);
    }
}