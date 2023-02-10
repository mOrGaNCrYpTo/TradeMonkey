using Microsoft.EntityFrameworkCore;

using TradeMonkey.Data.Entity;

public class TradeMonkeyDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=HP\\MFSQL;Database=MyDatabase;Trusted_Connection=True;TrustCertificate=true;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasMany(e => e.TraderGradesData)
                .WithOne(e => e.Token)
                .HasForeignKey(e => e.TokenId);
        });
    }
}