using NovoBanco.Domain.Enums;

namespace NovoBanco.Domain.Entities;

/// <summary>
/// Entidad que representa una transacción bancaria, con propiedades como el monto, tipo, referencia, estado y fecha de creación.
/// </summary>
public class Transaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid? DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string Reference { get; set; } = string.Empty;
    public TransactionStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public Account? Account { get; set; }
}