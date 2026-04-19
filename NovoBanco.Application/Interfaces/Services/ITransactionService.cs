using NovoBanco.Application.Dtos.Transactions;

namespace NovoBanco.Application.Interfaces.Services;

/// <summary>
/// Interfaz de servicio para manejar la lógica de negocio relacionada con las transacciones bancarias,
/// </summary>
public interface ITransactionService
{
    /// <summary>
    /// Deposita una cantidad específica en una cuenta bancaria
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DepositAsync(DepositRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retira una cantidad específica de una cuenta bancaria.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WithdrawAsync(WithdrawRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Transfiere una cantidad específica de una cuenta bancaria a otra cuenta bancaria.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task TransferAsync(TransferRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una lista paginada de transacciones asociadas a una cuenta bancaria específica.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<(IReadOnlyList<TransactionResponseDto> Items, int TotalCount)> GetAccountTransactionsAsync(
        Guid accountId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}