namespace NovoBanco.Application.Dtos.Customers;

/// <summary>
/// Representa los datos del cliente retornados por la aplicacion.
/// </summary>
public class CustomerResponseDto
{
    /// <summary>
    /// Identificador del cliente.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre completo del cliente.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Numero de documento del cliente.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;
}