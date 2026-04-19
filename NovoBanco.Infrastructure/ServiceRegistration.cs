using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NovoBanco.Application.Interfaces;
using NovoBanco.Application.Interfaces.Repositories;
using NovoBanco.Application.Interfaces.Services;
using NovoBanco.Application.Services;
using NovoBanco.Infrastructure.Data;
using NovoBanco.Infrastructure.Persistence;
using NovoBanco.Infrastructure.Repositories;

namespace NovoBanco.Infrastructure.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITransactionService, TransactionService>();

        return services;
    }
}