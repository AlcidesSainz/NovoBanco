using Microsoft.AspNetCore.Mvc;
using NovoBanco.Application.Dtos.Accounts;
using NovoBanco.Application.Interfaces.Services;

namespace NovoBanco.Api.Controllers;

/// <summary>
/// Controlador para manejo de cuentas bancarias
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ITransactionService _transactionService;

    public AccountsController(
        IAccountService accountService,
        ITransactionService transactionService)
    {
        _accountService = accountService;
        _transactionService = transactionService;
    }

    /// <summary>
    /// Crea una nueva cuenta bancaria para un cliente existente. 
    /// El número de cuenta se genera automáticamente y se garantiza su unicidad. 
    /// La cuenta se crea con un saldo inicial de 0 y un estado activo. 
    /// Se valida que el cliente exista antes de crear la cuenta. 
    /// Si el cliente no existe, se devuelve un error 404. 
    /// Si la creación es exitosa, se devuelve un código 201 con los detalles de la cuenta creada.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _accountService.CreateAccountAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Obtiene los detalles de una cuenta bancaria específica por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _accountService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene una lista paginada de transacciones asociadas a una cuenta bancaria específica.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}/transactions")]
    public async Task<IActionResult> GetTransactions(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _transactionService.GetAccountTransactionsAsync(id, page, pageSize, cancellationToken);
        return Ok(new
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = result.TotalCount,
            Items = result.Items
        });
    }

    /// <summary>
    /// Bloquea una cuenta bancaria.
    /// </summary>
    /// <param name="id">Identificador de la cuenta.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}/block")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BlockAccount(Guid id, CancellationToken cancellationToken)
    {
        await _accountService.BlockAccountAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Cerrar una cuenta bancaria.
    /// </summary>
    /// <param name="id">Identificador de la cuenta.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}/close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CloseAccount(Guid id, CancellationToken cancellationToken)
    {
        await _accountService.CloseAccountAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Activar una cuenta bancaria.
    /// </summary>
    /// <param name="id">Identificador de la cuenta.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActivateAccount(Guid id, CancellationToken cancellationToken)
    {
        await _accountService.ActivateAccountAsync(id, cancellationToken);
        return NoContent();
    }
}