namespace NovoBanco.Domain.Exceptions;

public class InactiveAccountException : BusinessException
{
    public InactiveAccountException()
        : base("The account is blocked or closed and cannot operate.")
    {
    }
}