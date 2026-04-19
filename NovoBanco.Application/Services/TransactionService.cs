using NovoBanco.Application.Dtos.Transactions;
using NovoBanco.Application.Interfaces;
using NovoBanco.Application.Interfaces.Repositories;
using NovoBanco.Application.Interfaces.Services;
using NovoBanco.Application.Resources;
using NovoBanco.Domain.Entities;
using NovoBanco.Domain.Enums;
using NovoBanco.Domain.Exceptions;

namespace NovoBanco.Application.Services;

/// <summary>
/// Maneja las operaciones de negocio relacionadas con las transacciones.
/// </summary>
public class TransactionService : ITransactionService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TransactionService"/>.
    /// </summary>
    public TransactionService(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deposita un monto específico en una cuenta bancaria.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="BusinessException"></exception>
    public async Task DepositAsync(DepositRequestDto request, CancellationToken cancellationToken = default)
    {
        // Validación de monto positivo
        if (request.Amount <= 0)
        {
            throw new ArgumentException(Resources.ValidationMessages.Amount_Must_Be_Greater_Than_Zero);
        }

        // Obtener la cuenta
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);

        if (account is null)
        {
            throw new KeyNotFoundException(Resources.ErrorMessages.Account_NotFound);
        }

        // Validar que la cuenta esté activa
        EnsureAccountIsActive(account);

        // Aplicar lógica de negocio
        account.Balance += request.Amount;

        // Registrar transacción
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            Amount = request.Amount,
            Type = TransactionType.Deposit,
            Reference = Guid.NewGuid().ToString(),
            Status = TransactionStatus.Successful,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _transactionRepository.AddAsync(transaction, cancellationToken);
        await _accountRepository.UpdateAsync(account, cancellationToken);

        // Persistir cambios
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Metodo para retirar un monto específico de una cuenta bancaria. 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="BusinessException"></exception>
    /// <exception cref="InsufficientFundsException"></exception>
    public async Task WithdrawAsync(WithdrawRequestDto request, CancellationToken cancellationToken = default)
    {
        // Validación de monto positivo
        if (request.Amount <= 0)
        {
            throw new ArgumentException(Resources.ValidationMessages.Amount_Must_Be_Greater_Than_Zero);
        }

        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);

        if (account is null)
        {
            throw new KeyNotFoundException(Resources.ErrorMessages.Account_NotFound);
        }

        // Validar estado de la cuenta
        EnsureAccountIsActive(account);

        // Validar saldo suficiente
        if (account.Balance < request.Amount)
        {
            throw new InsufficientFundsException();
        }

        // Aplicar lógica
        account.Balance -= request.Amount;

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            Amount = request.Amount,
            Type = TransactionType.Withdrawal,
            Reference = Guid.NewGuid().ToString(),
            Status = TransactionStatus.Successful,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _transactionRepository.AddAsync(transaction, cancellationToken);
        await _accountRepository.UpdateAsync(account, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Transfiere un monto específico de una cuenta bancaria a otra cuenta bancaria.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="BusinessException"></exception>
    public async Task TransferAsync(TransferRequestDto request, CancellationToken cancellationToken = default)
    {
        // Validaciones básicas
        if (request.Amount <= 0)
        {
            throw new ArgumentException(Resources.ValidationMessages.Amount_Must_Be_Greater_Than_Zero);
        }

        if (request.SourceAccountId == request.DestinationAccountId)
        {
            throw new ArgumentException(ValidationMessages.Same_Account_Transfer);
        }

        // Iniciar transacción de base de datos
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var sourceAccount = await _accountRepository.GetByIdAsync(request.SourceAccountId, cancellationToken);
            var destinationAccount = await _accountRepository.GetByIdAsync(request.DestinationAccountId, cancellationToken);

            if (sourceAccount is null)
                throw new KeyNotFoundException(ErrorMessages.Source_Account_NotFound);

            if (destinationAccount is null)
                throw new KeyNotFoundException(ErrorMessages.Destination_Account_NotFound);

            // Validar cuentas activas
            EnsureAccountIsActive(sourceAccount);
            EnsureAccountIsActive(destinationAccount);

            // Validar saldo
            if (sourceAccount.Balance < request.Amount)
                throw new InsufficientFundsException();

            // Aplicar movimientos
            sourceAccount.Balance -= request.Amount;
            destinationAccount.Balance += request.Amount;
            string baseReference = Guid.NewGuid().ToString();

            // Registro de débito
            var debitTransaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = sourceAccount.Id,
                DestinationAccountId = destinationAccount.Id,
                Amount = request.Amount,
                Type = TransactionType.TransferDebit,
                Reference = Guid.NewGuid().ToString(),
                Status = TransactionStatus.Successful,
                CreatedAtUtc = DateTime.UtcNow
            };

            // Registro de crédito
            var creditTransaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = destinationAccount.Id,
                DestinationAccountId = sourceAccount.Id,
                Amount = request.Amount,
                Type = TransactionType.TransferCredit,
                Reference = $"{baseReference}-IN",
                Status = TransactionStatus.Successful,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _accountRepository.UpdateAsync(sourceAccount, cancellationToken);
            await _accountRepository.UpdateAsync(destinationAccount, cancellationToken);

            await _transactionRepository.AddAsync(debitTransaction, cancellationToken);
            await _transactionRepository.AddAsync(creditTransaction, cancellationToken);

            // Guardar y confirmar transacción
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            // Revertir en caso de error
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    /// <summary>
    /// Obtiene una lista paginada de transacciones asociadas a una cuenta bancaria específica.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<(IReadOnlyList<TransactionResponseDto> Items, int TotalCount)> GetAccountTransactionsAsync(
        Guid accountId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);

        if (account is null)
        {
            throw new KeyNotFoundException(Resources.ErrorMessages.Account_NotFound);
        }

        var result = await _transactionRepository.GetPagedByAccountIdAsync(accountId, page, pageSize, cancellationToken);

        var items = result.Items.Select(x => new TransactionResponseDto
        {
            Id = x.Id,
            AccountId = x.AccountId,
            DestinationAccountId = x.DestinationAccountId,
            Amount = x.Amount,
            Type = x.Type.ToString(),
            Reference = x.Reference,
            Status = x.Status.ToString(),
            CreatedAtUtc = x.CreatedAtUtc
        }).ToList();

        return (items, result.TotalCount);
    }

    /// <summary>
    /// Valida que la cuenta esté en estado activo.
    /// </summary>
    private static void EnsureAccountIsActive(Account account)
    {
        if (account.Status != AccountStatus.Active)
        {
            throw new InactiveAccountException();
        }
    }
}