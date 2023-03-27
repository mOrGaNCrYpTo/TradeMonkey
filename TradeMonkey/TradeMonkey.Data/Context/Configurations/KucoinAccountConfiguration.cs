namespace TradeMonkey.Data.Context.Configurations
{
    public partial class KucoinAccountConfiguration : IEntityTypeConfiguration<KucoinAccount>
    {
        public void Configure(EntityTypeBuilder<KucoinAccount> entity)
        {
            entity.ToTable("Kucoin_Accounts");

            entity.Property(e => e.Id)
            .HasMaxLength(50)
            .IsUnicode(false);
            entity.Property(e => e.Asset)
            .IsRequired()
            .HasMaxLength(10)
            .IsUnicode(false);
            entity.Property(e => e.Available)
            .HasDefaultValueSql("((0.00))")
            .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Total)
            .HasDefaultValueSql("((0.00))")
            .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Type)
            .IsRequired()
            .HasMaxLength(10)
            .IsUnicode(false);
            entity.Property(e => e.Updated)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<KucoinAccount> entity);
    }
}