using NovoBanco.Domain.Entities;

namespace NovoBanco.Application.Interfaces.Repositories;

/// <summary>
/// Interfaz de repositorio para la entidad Transaction, 
/// que define los métodos necesarios para acceder y 
/// manipular los datos de las transacciones bancarias en la capa de persistencia.
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Agrega una nueva transacción a la base de datos.
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);

    /// <summary>
    /// Encuentra una transacción por su referencia única
    /// </summary>
    /// <param name="reference"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ExistsByReferenceAsync(string reference, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una lista paginada de transacciones asociadas a una cuenta bancaria específica,
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPagedByAccountIdAsync(
        Guid accountId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}