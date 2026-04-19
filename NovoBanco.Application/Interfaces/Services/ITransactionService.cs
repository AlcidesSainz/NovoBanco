using NovoBanco.Application.Dtos.Transactions;

namespace NovoBanco.Application.Interfaces.Services;

public interface ITransactionService
{
    Task DepositAsync(DepositRequestDto request, CancellationToken cancellationToken = default);
    Task WithdrawAsync(WithdrawRequestDto request, CancellationToken cancellationToken = default);
    Task TransferAsync(TransferRequestDto request, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<TransactionResponseDto> Items, int TotalCount)> GetAccountTransactionsAsync(
        Guid accountId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}