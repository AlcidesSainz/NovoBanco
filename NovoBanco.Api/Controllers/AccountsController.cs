using Microsoft.AspNetCore.Mvc;
using NovoBanco.Application.Dtos.Accounts;
using NovoBanco.Application.Interfaces.Services;

namespace NovoBanco.Api.Controllers;

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

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _accountService.CreateAccountAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _accountService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

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
}