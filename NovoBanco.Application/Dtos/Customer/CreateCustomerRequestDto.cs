namespace NovoBanco.Application.Dtos.Customers;

/// <summary>
/// Representa los datos necesarios para crear un cliente.
/// </summary>
public class CreateCustomerRequestDto
{
    /// <summary>
    /// Nombre completo del cliente.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Numero de documento del cliente.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;
}