using NovoBanco.Domain.Entities;

namespace NovoBanco.Application.Interfaces.Repositories;

/// <summary>
/// Interfaz de repositorio para la entidad Account, 
/// que define los métodos necesarios para acceder y 
/// manipular los datos de las cuentas bancarias en la capa de persistencia.
/// </summary>
public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
    Task AddAsync(Account account, CancellationToken cancellationToken = default);
    Task UpdateAsync(Account account, CancellationToken cancellationToken = default);
}