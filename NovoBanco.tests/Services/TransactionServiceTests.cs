using FluentAssertions;
using Moq;
using NovoBanco.Application.Dtos.Transactions;
using NovoBanco.Application.Interfaces;
using NovoBanco.Application.Interfaces.Repositories;
using NovoBanco.Application.Services;
using NovoBanco.Domain.Entities;
using NovoBanco.Domain.Enums;
using NovoBanco.Domain.Exceptions;
using System.Security.Principal;

namespace NovoBanco.Tests.Services;

public class TransactionServiceTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly TransactionService _service;

    public TransactionServiceTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _service = new TransactionService(
            _accountRepositoryMock.Object,
            _transactionRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task DepositAsync_Should_Increase_Balance_When_Request_Is_Valid()
    {
        // Arrange
        var account = new Account
        {
            Id = Guid.NewGuid(),
            Balance = 100m,
            Status = AccountStatus.Active
        };

        var request = new DepositRequestDto
        {
            AccountId = account.Id,
            Amount = 50m
        };

        _accountRepositoryMock
            .Setup(x => x.GetByIdAsync(account.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        _transactionRepositoryMock
            .Setup(x => x.ExistsByReferenceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        await _service.DepositAsync(request, CancellationToken.None);

        // Assert
        account.Balance.Should().Be(150m);

        _transactionRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _accountRepositoryMock.Verify(
            x => x.UpdateAsync(account, It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task WithdrawAsync_Should_Throw_When_Balance_Is_Insufficient()
    {
        // Arrange
        var account = new Account
        {
            Id = Guid.NewGuid(),
            Balance = 20m,
            Status = AccountStatus.Active
        };

        var request = new WithdrawRequestDto
        {
            AccountId = account.Id,
            Amount = 50m
        };

        _accountRepositoryMock
            .Setup(x => x.GetByIdAsync(account.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        _transactionRepositoryMock
            .Setup(x => x.ExistsByReferenceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var act = async () => await _service.WithdrawAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InsufficientFundsException>();

        account.Balance.Should().Be(20m);

        _transactionRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task TransferAsync_Should_Throw_When_Source_And_Destination_Are_Equal()
    {
        // Arrange
        var sameId = Guid.NewGuid();

        var request = new TransferRequestDto
        {
            SourceAccountId = sameId,
            DestinationAccountId = sameId,
            Amount = 10m
        };

        // Act
        var act = async () => await _service.TransferAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task DepositAsync_Should_Throw_When_Account_Is_Blocked()
    {
        // Arrange
        var account = new Account
        {
            Id = Guid.NewGuid(),
            Balance = 100m,
            Status = AccountStatus.Blocked
        };

        var request = new DepositRequestDto
        {
            AccountId = account.Id,
            Amount = 25m
        };

        _accountRepositoryMock
            .Setup(x => x.GetByIdAsync(account.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        // Act
        var act = async () => await _service.DepositAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InactiveAccountException>();

        _transactionRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }
}