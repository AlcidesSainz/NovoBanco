namespace NovoBanco.Domain.Exceptions;

public class InsufficientFundsException : BusinessException
{
    public InsufficientFundsException()
        : base("The account does not have sufficient funds.")
    {
    }
}