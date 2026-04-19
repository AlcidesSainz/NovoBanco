using NovoBanco.Application.Dtos.Accounts;

namespace NovoBanco.Application.Interfaces.Services;
/// <summary>
/// Interfaz de servicio para la entidad Account,
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Crea una nueva cuenta bancaria
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AccountResponseDto> CreateAccountAsync(CreateAccountRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene los detalles de una cuenta bancaria específica por su ID.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AccountResponseDto> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bloquear cuenta a traves del id
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task BlockAccountAsync(Guid accountId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cerrar cuenta a traves del id
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CloseAccountAsync(Guid accountId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Activar cuenta a traves del id
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ActivateAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
}