namespace NovoBanco.Domain.Exceptions;

/// <summary>
/// Clase de excepción personalizada que representa un error de negocio relacionado con una cuenta bancaria inactiva, bloqueada o cerrada,
/// </summary>
public class InactiveAccountException : BusinessException
{
    public InactiveAccountException()
        : base("The account is blocked or closed and cannot operate.")
    {
    }
}