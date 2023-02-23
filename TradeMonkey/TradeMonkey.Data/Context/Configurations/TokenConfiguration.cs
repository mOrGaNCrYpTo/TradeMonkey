namespace TradeMonkey.Data.Context.Configurations
{
    public partial class TokenConfiguration : IEntityTypeConfiguration<TokenMetricsTokens>
    {
        partial void OnConfigurePartial(EntityTypeBuilder<TokenMetricsTokens> entity);

        public void Configure(EntityTypeBuilder<TokenMetricsTokens> entity)
        {
            entity.HasKey(e => e.Token_Id).HasName("PK__Token__658FEEEACA5CFF1A");

            entity.Property(e => e.Token_Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Symbol)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            OnConfigurePartial(entity);
        }
    }
}