namespace NovoBanco.Domain.Exceptions;

/// <summary>
/// Clase de excepción personalizada para representar errores de negocio en la aplicación,
/// </summary>
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }
}