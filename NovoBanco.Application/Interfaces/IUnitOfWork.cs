namespace NovoBanco.Application.Interfaces;

/// <summary>
/// Define una unidad de trabajo para coordinar la persistencia de datos
/// y las transacciones de base de datos.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Guarda todos los cambios pendientes.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Inicia una transacción en la base de datos.
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirma (commit) la transacción actual en la base de datos.
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Revierte (rollback) la transacción actual en la base de datos.
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}