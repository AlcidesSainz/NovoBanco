namespace NovoBanco.Domain.Enums;

/// <summary>
/// Enum que representa el tipo de transacción bancaria, con valores como Depósito, Retiro, Transferencia Débito y Transferencia Crédito.
/// </summary>
public enum TransactionType
{
    Deposit = 1,
    Withdrawal = 2,
    TransferDebit = 3,
    TransferCredit = 4
}