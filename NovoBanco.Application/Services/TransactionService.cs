using NovoBanco.Application.Dtos.Transactions;
using NovoBanco.Application.Interfaces;
using NovoBanco.Application.Interfaces.Repositories;
using NovoBanco.Application.Interfaces.Services;
using NovoBanco.Domain.Entities;
using NovoBanco.Domain.Enums;
using NovoBanco.Domain.Exceptions;

namespace NovoBanco.Application.Services;

/// <summary>
/// Handles transaction-related business operations.
/// </summary>
public class TransactionService : ITransactionService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionService"/> class.
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

    /// <inheritdoc />
    public async Task DepositAsync(DepositRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }

        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);

        if (account is null)
        {
            throw new KeyNotFoundException("Account was not found.");
        }

        EnsureAccountIsActive(account);

        var referenceExists = await _transactionRepository.ExistsByReferenceAsync(request.Reference, cancellationToken);

        if (referenceExists)
        {
            throw new BusinessException("A transaction with the same reference already exists.");
        }

        account.Balance += request.Amount;

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            Amount = request.Amount,
            Type = TransactionType.Deposit,
            Reference = request.Reference,
            Status = TransactionStatus.Successful,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _transactionRepository.AddAsync(transaction, cancellationToken);
        await _accountRepository.UpdateAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task WithdrawAsync(WithdrawRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }

        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);

        if (account is null)
        {
            throw new KeyNotFoundException("Account was not found.");
        }

        EnsureAccountIsActive(account);

        var referenceExists = await _transactionRepository.ExistsByReferenceAsync(request.Reference, cancellationToken);

        if (referenceExists)
        {
            throw new BusinessException("A transaction with the same reference already exists.");
        }

        if (account.Balance < request.Amount)
        {
            throw new InsufficientFundsException();
        }

        account.Balance -= request.Amount;

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            Amount = request.Amount,
            Type = TransactionType.Withdrawal,
            Reference = request.Reference,
            Status = TransactionStatus.Successful,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _transactionRepository.AddAsync(transaction, cancellationToken);
        await _accountRepository.UpdateAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task TransferAsync(TransferRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }

        if (request.SourceAccountId == request.DestinationAccountId)
        {
            throw new ArgumentException("Source and destination accounts must be different.");
        }

        var referenceExists = await _transactionRepository.ExistsByReferenceAsync(request.Reference, cancellationToken);

        if (referenceExists)
        {
            throw new BusinessException("A transaction with the same reference already exists.");
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var sourceAccount = await _accountRepository.GetByIdAsync(request.SourceAccountId, cancellationToken);
            var destinationAccount = await _accountRepository.GetByIdAsync(request.DestinationAccountId, cancellationToken);

            if (sourceAccount is null)
            {
                throw new KeyNotFoundException("Source account was not found.");
            }

            if (destinationAccount is null)
            {
                throw new KeyNotFoundException("Destination account was not found.");
            }

            EnsureAccountIsActive(sourceAccount);
            EnsureAccountIsActive(destinationAccount);

            if (sourceAccount.Balance < request.Amount)
            {
                throw new InsufficientFundsException();
            }

            sourceAccount.Balance -= request.Amount;
            destinationAccount.Balance += request.Amount;

            var debitTransaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = sourceAccount.Id,
                DestinationAccountId = destinationAccount.Id,
                Amount = request.Amount,
                Type = TransactionType.TransferDebit,
                Reference = request.Reference,
                Status = TransactionStatus.Successful,
                CreatedAtUtc = DateTime.UtcNow
            };

            var creditTransaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = destinationAccount.Id,
                DestinationAccountId = sourceAccount.Id,
                Amount = request.Amount,
                Type = TransactionType.TransferCredit,
                Reference = $"{request.Reference}-IN",
                Status = TransactionStatus.Successful,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _accountRepository.UpdateAsync(sourceAccount, cancellationToken);
            await _accountRepository.UpdateAsync(destinationAccount, cancellationToken);

            await _transactionRepository.AddAsync(debitTransaction, cancellationToken);
            await _transactionRepository.AddAsync(creditTransaction, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<TransactionResponseDto> Items, int TotalCount)> GetAccountTransactionsAsync(
        Guid accountId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);

        if (account is null)
        {
            throw new KeyNotFoundException("Account was not found.");
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

    private static void EnsureAccountIsActive(Account account)
    {
        if (account.Status != AccountStatus.Active)
        {
            throw new InactiveAccountException();
        }
    }
}