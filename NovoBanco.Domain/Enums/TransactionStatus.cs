namespace NovoBanco.Domain.Enums;

/// <summary>
/// Enum que representa el estado de una transacción bancaria, con valores como Exitosa, Fallida y Revertida.
/// </summary>
public enum TransactionStatus
{
    Successful = 1,
    Failed = 2,
    Reverted = 3
}