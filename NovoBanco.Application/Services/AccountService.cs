using NovoBanco.Application.Dtos.Accounts;
using NovoBanco.Application.Interfaces;
using NovoBanco.Application.Interfaces.Repositories;
using NovoBanco.Application.Interfaces.Services;
using NovoBanco.Domain.Entities;
using NovoBanco.Domain.Enums;

namespace NovoBanco.Application.Services;

/// <summary>
/// Maneja las operaciones de negocio relacionadas con las cuentas.
/// </summary>
public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AccountService"/>.
    /// </summary>
    public AccountService(IAccountRepository accountRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Crea una nueva cuenta bancaria para un cliente existente.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<AccountResponseDto> CreateAccountAsync(CreateAccountRequestDto request, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            throw new KeyNotFoundException("Customer was not found.");
        }

        var account = new Account
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            AccountNumber = GenerateAccountNumber(),
            Type = (AccountType)request.AccountType,
            Currency = "USD",
            Balance = 0m,
            Status = AccountStatus.Active,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _accountRepository.AddAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AccountResponseDto
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            AccountType = account.Type.ToString(),
            Currency = account.Currency,
            Balance = account.Balance,
            Status = account.Status.ToString(),
            CustomerId = account.CustomerId
        };
    }

    /// <summary>
    /// Obtiene los detalles de una cuenta bancaria específica por su ID.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<AccountResponseDto> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);

        if (account is null)
        {
            throw new KeyNotFoundException("Account was not found.");
        }

        return new AccountResponseDto
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            AccountType = account.Type.ToString(),
            Currency = account.Currency,
            Balance = account.Balance,
            Status = account.Status.ToString(),
            CustomerId = account.CustomerId
        };
    }

    /// <summary>
    /// Genera un número de cuenta aleatorio.
    /// </summary>
    /// <returns>Número de cuenta generado.</returns>
    private static string GenerateAccountNumber()
    {
        var random = new Random();
        return $"{random.Next(10000000, 99999999)}";
    }
}