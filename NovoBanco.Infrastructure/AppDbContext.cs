using Microsoft.EntityFrameworkCore;
using NovoBanco.Domain.Entities;

namespace NovoBanco.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Clientes");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("Id");
            entity.Property(x => x.FullName)
                .HasColumnName("NombreCompleto")
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(x => x.DocumentNumber)
                .HasColumnName("NumeroDocumento")
                .HasMaxLength(50)
                .IsRequired();
            entity.HasIndex(x => x.DocumentNumber)
                .IsUnique()
                .HasDatabaseName("IX_Clientes_NumeroDocumento");
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Cuentas");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("Id");

            entity.Property(x => x.AccountNumber)
                .HasColumnName("NumeroCuenta")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.CustomerId)
                .HasColumnName("ClienteId");

            entity.Property(x => x.Currency)
                .HasColumnName("Moneda")
                .HasMaxLength(3)
                .IsRequired();

            entity.Property(x => x.Balance)
                .HasColumnName("Saldo")
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.Status)
                .HasColumnName("Estado");

            entity.Property(x => x.CreatedAtUtc)
                .HasColumnName("FechaCreacion");

            entity.Property(x => x.RowVersion)
                .HasColumnName("RowVersion")
                .IsRowVersion();

            entity.HasIndex(x => x.AccountNumber)
                .IsUnique()
                .HasDatabaseName("IX_Cuentas_NumeroCuenta");

            entity.HasIndex(x => x.CustomerId)
                .HasDatabaseName("IX_Cuentas_ClienteId");

            entity.HasOne(x => x.Customer)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.CustomerId)
                .HasConstraintName("FK_Cuentas_Clientes");

            entity.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Cuentas_Saldo_NoNegativo", "[Saldo] >= 0");
            });
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transacciones");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("Id");

            entity.Property(x => x.AccountId)
                .HasColumnName("CuentaId");

            entity.Property(x => x.DestinationAccountId)
                .HasColumnName("CuentaDestinoId");

            entity.Property(x => x.Amount)
                .HasColumnName("Monto")
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.Reference)
                .HasColumnName("Referencia")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Type)
                .HasColumnName("Tipo");

            entity.Property(x => x.Status)
                .HasColumnName("Estado");

            entity.Property(x => x.CreatedAtUtc)
                .HasColumnName("FechaCreacion");

            entity.HasIndex(x => x.Reference)
                .IsUnique()
                .HasDatabaseName("IX_Transacciones_Referencia");

            entity.HasIndex(x => new { x.AccountId, x.CreatedAtUtc })
                .HasDatabaseName("IX_Transacciones_Cuenta_Fecha");

            entity.HasOne(x => x.Account)
                .WithMany()
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Transacciones_Cuentas");
        });
    }
}