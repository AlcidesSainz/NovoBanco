using Microsoft.EntityFrameworkCore;
using NovoBanco.Application.Interfaces.Repositories;
using NovoBanco.Domain.Entities;
using NovoBanco.Infrastructure.Data;

namespace NovoBanco.Infrastructure.Repositories;

/// <summary>
/// Implementa las operaciones de acceso a datos para clientes.
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Inicializa una nueva instancia de la clase CustomerRepository.
    /// </summary>
    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Customer?> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(x => x.DocumentNumber == documentNumber, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await _context.Customers.AddAsync(customer, cancellationToken);
    }
}