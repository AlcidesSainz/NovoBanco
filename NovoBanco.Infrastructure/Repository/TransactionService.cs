using Microsoft.EntityFrameworkCore;
using NovoBanco.Application.Interfaces.Repositories;
using NovoBanco.Domain.Entities;
using NovoBanco.Infrastructure.Data;

namespace NovoBanco.Infrastructure.Repositories;

/// <summary>
/// Clase de repositorio para la entidad Transaction, 
/// que implementa la interfaz ITransactionRepository y 
/// proporciona los métodos necesarios para acceder y 
/// manipular los datos de las transacciones bancarias en 
/// la capa de persistencia utilizando Entity Framework Core.
/// </summary>
public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        await _context.Transactions.AddAsync(transaction, cancellationToken);
    }

    public async Task<bool> ExistsByReferenceAsync(string reference, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .AnyAsync(x => x.Reference == reference, cancellationToken);
    }

    public async Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPagedByAccountIdAsync(
        Guid accountId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Transactions
            .Where(x => x.AccountId == accountId)
            .OrderByDescending(x => x.CreatedAtUtc);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}