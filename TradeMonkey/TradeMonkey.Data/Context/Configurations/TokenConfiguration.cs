namespace TradeMonkey.Data.Context.Configurations
{
    public partial class TokenConfiguration : IEntityTypeConfiguration<Tokens>
    {
        partial void OnConfigurePartial(EntityTypeBuilder<Tokens> entity);

        public void Configure(EntityTypeBuilder<Tokens> entity)
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