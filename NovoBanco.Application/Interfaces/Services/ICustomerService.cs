using NovoBanco.Application.Dtos.Customers;

namespace NovoBanco.Application.Interfaces.Services;

/// <summary>
/// Define las operaciones de negocio relacionadas con clientes.
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Crea un nuevo cliente.
    /// </summary>
    Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un cliente por su identificador.
    /// </summary>
    Task<CustomerResponseDto> GetByIdAsync(Guid customerId, CancellationToken cancellationToken = default);
}