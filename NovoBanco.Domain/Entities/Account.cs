using NovoBanco.Domain.Enums;

namespace NovoBanco.Domain.Entities;

public class Account
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public AccountType Type { get; set; }
    public string Currency { get; set; } = "USD";
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public Customer? Customer { get; set; }
}