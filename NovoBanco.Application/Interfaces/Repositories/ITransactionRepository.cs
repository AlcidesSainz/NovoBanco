using NovoBanco.Domain.Entities;

namespace NovoBanco.Application.Interfaces.Repositories;

/// <summary>
/// Interfaz de repositorio para la entidad Transaction, 
/// que define los métodos necesarios para acceder y 
/// manipular los datos de las transacciones bancarias en la capa de persistencia.
/// </summary>
public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
    Task<bool> ExistsByReferenceAsync(string reference, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPagedByAccountIdAsync(
        Guid accountId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}