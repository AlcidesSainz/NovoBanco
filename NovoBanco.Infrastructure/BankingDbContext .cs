using Microsoft.EntityFrameworkCore;

public class BankingDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public BankingDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .Property(a => a.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.Reference)
            .IsUnique();
    }

}