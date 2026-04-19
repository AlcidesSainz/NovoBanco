namespace NovoBanco.Application.Dtos.Accounts;
/// <summary>
/// Dto de solicitud para crear una nueva cuenta bancaria, que incluye el ID del cliente al que se asociará la cuenta y el tipo de cuenta a crear.
/// </summary>
public class CreateAccountRequestDto
{
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Campo tipo de cuenta Ahorros(1) y Corriente(2)
    /// </summary>
    public int AccountType { get; set; }
}