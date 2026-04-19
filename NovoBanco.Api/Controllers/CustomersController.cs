using Microsoft.AspNetCore.Mvc;
using NovoBanco.Application.Dtos.Customers;
using NovoBanco.Application.Interfaces.Services;

namespace NovoBanco.Api.Controllers;

/// <summary>
/// Expone endpoints para la gestion de clientes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    /// <summary>
    /// Inicializa una nueva instancia del controller.
    /// </summary>
    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Crea un nuevo cliente.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CustomerResponseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCustomerRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _customerService.CreateCustomerAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Obtiene un cliente por su identificador.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CustomerResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _customerService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }
}