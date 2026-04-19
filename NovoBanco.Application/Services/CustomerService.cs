using NovoBanco.Application.Dtos.Customers;
using NovoBanco.Application.Interfaces;
using NovoBanco.Application.Interfaces.Repositories;
using NovoBanco.Application.Interfaces.Services;
using NovoBanco.Domain.Entities;
using NovoBanco.Domain.Exceptions;

namespace NovoBanco.Application.Services;

/// <summary>
/// Maneja las operaciones de negocio relacionadas con clientes.
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia de la clase CustomerService.
    /// </summary>
    public CustomerService(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Metodo para crear un nuevo cliente. Valida los datos de entrada,
    /// verifica que no exista un cliente con el mismo número de documento.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="BusinessException"></exception>
    public async Task<CustomerResponseDto> CreateCustomerAsync(
        CreateCustomerRequestDto request,
        CancellationToken cancellationToken = default)
    {
        // Validacion de nombre
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            throw new ArgumentException("Full name is required.");
        }

        // Validacion de documento
        if (string.IsNullOrWhiteSpace(request.DocumentNumber))
        {
            throw new ArgumentException("Document number is required.");
        }

        // Validar duplicado por documento
        var existingCustomer = await _customerRepository
            .GetByDocumentNumberAsync(request.DocumentNumber, cancellationToken);

        if (existingCustomer is not null)
        {
            throw new BusinessException("A customer with the same document number already exists.");
        }

        // Crear entidad
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            DocumentNumber = request.DocumentNumber.Trim()
        };

        // Persistir
        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar DTO
        return new CustomerResponseDto
        {
            Id = customer.Id,
            FullName = customer.FullName,
            DocumentNumber = customer.DocumentNumber
        };
    }

    /// <summary>
    /// Obtiene los detalles de un cliente específico por su ID. Si el cliente no existe, se lanza una excepción KeyNotFoundException.
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<CustomerResponseDto> GetByIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);

        if (customer is null)
        {
            throw new KeyNotFoundException("Customer was not found.");
        }

        return new CustomerResponseDto
        {
            Id = customer.Id,
            FullName = customer.FullName,
            DocumentNumber = customer.DocumentNumber
        };
    }
}