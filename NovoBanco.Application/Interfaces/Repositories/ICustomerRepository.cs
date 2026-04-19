using NovoBanco.Domain.Entities;

namespace NovoBanco.Application.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de persistencia para clientes.
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Obtiene un cliente por su identificador.
    /// </summary>
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un cliente por numero de documento.
    /// </summary>
    Task<Customer?> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega un nuevo cliente.
    /// </summary>
    Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
}