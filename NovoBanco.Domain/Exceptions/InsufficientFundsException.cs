namespace NovoBanco.Domain.Exceptions;

/// <summary>
/// Clase de excepción personalizada que representa un error de fondos insuficientes en una cuenta bancaria,
/// </summary>
public class InsufficientFundsException : BusinessException
{
    public InsufficientFundsException()
        : base("La cuenta no tiene saldo suficiente.")
    {
    }
}