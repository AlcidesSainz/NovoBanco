using NovoBanco.Domain.Entities;

namespace NovoBanco.Application.Interfaces.Repositories;

/// <summary>
/// Interfaz de repositorio para la entidad Account, 
/// que define los métodos necesarios para acceder y 
/// manipular los datos de las cuentas bancarias en la capa de persistencia.
/// </summary>
public interface IAccountRepository
{
    /// <summary>
    /// Obtiene una cuenta bancaria por su identificador único (ID).
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtene una cuenta bancaria por su número de cuenta, que es un valor único asignado a cada cuenta.
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Anade una nueva cuenta bancaria a la base de datos.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddAsync(Account account, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza los datos de una cuenta bancaria existente en la base de datos.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateAsync(Account account, CancellationToken cancellationToken = default);
}