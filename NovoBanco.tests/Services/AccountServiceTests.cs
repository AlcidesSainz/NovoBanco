using FluentAssertions;
using Moq;
using NovoBanco.Application.Dtos.Accounts;
using NovoBanco.Application.Interfaces;
using NovoBanco.Application.Interfaces.Repositories;
using NovoBanco.Application.Services;
using NovoBanco.Domain.Entities;

namespace NovoBanco.Tests.Services;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AccountService _service;

    public AccountServiceTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _service = new AccountService(
            _accountRepositoryMock.Object,
            _customerRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateAccountAsync_Should_Create_Account_When_Customer_Exists()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var customer = new Customer
        {
            Id = customerId,
            FullName = "Juan Perez",
            DocumentNumber = "1234567890"
        };

        var request = new CreateAccountRequestDto
        {
            CustomerId = customerId,
            AccountType = 1
        };

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        // Act
        var result = await _service.CreateAccountAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CustomerId.Should().Be(customerId);
        result.Currency.Should().Be("USD");
        result.Balance.Should().Be(0m);

        _accountRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Throw_When_Account_Does_Not_Exist()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        _accountRepositoryMock
            .Setup(x => x.GetByIdAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Account?)null);

        // Act
        var act = async () => await _service.GetByIdAsync(accountId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}