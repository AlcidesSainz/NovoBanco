namespace NovoBanco.Application.Dtos.Accounts;

/// <summary>
/// Dto de respuesta para la entidad Cuenta, utilizado para devolver los detalles de una cuenta bancaria en las respuestas de la API.
/// </summary>
public class AccountResponseDto
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
}