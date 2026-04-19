using NovoBanco.Domain.Entities;

namespace NovoBanco.Application.Interfaces.Repositories;

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