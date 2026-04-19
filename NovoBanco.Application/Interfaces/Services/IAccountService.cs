using NovoBanco.Application.Dtos.Accounts;

namespace NovoBanco.Application.Interfaces.Services;

public interface IAccountService
{
    Task<AccountResponseDto> CreateAccountAsync(CreateAccountRequestDto request, CancellationToken cancellationToken = default);
    Task<AccountResponseDto> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default);
}