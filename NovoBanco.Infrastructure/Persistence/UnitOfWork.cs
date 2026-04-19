using Microsoft.EntityFrameworkCore.Storage;
using NovoBanco.Application.Interfaces;
using NovoBanco.Infrastructure.Data;

namespace NovoBanco.Infrastructure.Persistence;

/// <summary>
/// Coordina las operaciones de persistencia y los límites de transacciones de base de datos.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UnitOfWork"/>.
    /// </summary>
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Guarda todos los cambios pendientes en la base de datos
        return await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Metodo para iniciar una transacción en la base de datos. Si ya existe una transacción activa, no hace nada.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        // Evita iniciar una nueva transacción si ya existe una activa
        if (_transaction is not null)
        {
            return;
        }

        // Inicia una nueva transacción en la base de datos
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Metodo para confirmar la transacción actual en la base de datos
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        // Si no hay transacción activa, no hace nada
        if (_transaction is null)
        {
            return;
        }

        // Confirma la transacción y libera recursos
        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    /// <summary>
    /// Metodo para revertir la transacción actual en la base de datos en caso de error o cancel
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        // Si no hay transacción activa, no hace nada
        if (_transaction is null)
        {
            return;
        }

        // Revierte la transacción y libera recursos
        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }
}