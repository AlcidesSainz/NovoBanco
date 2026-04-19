using Microsoft.EntityFrameworkCore;
using NovoBanco.Application.Interfaces.Repositories;
using NovoBanco.Domain.Entities;
using NovoBanco.Infrastructure.Data;

namespace NovoBanco.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}