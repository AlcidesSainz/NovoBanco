using Microsoft.AspNetCore.Mvc;
using NovoBanco.Application.Dtos.Transactions;
using NovoBanco.Application.Interfaces.Services;

namespace NovoBanco.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(
        [FromBody] DepositRequestDto request,
        CancellationToken cancellationToken)
    {
        await _transactionService.DepositAsync(request, cancellationToken);
        return Ok(new { message = "Deposit processed successfully." });
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(
        [FromBody] WithdrawRequestDto request,
        CancellationToken cancellationToken)
    {
        await _transactionService.WithdrawAsync(request, cancellationToken);
        return Ok(new { message = "Withdrawal processed successfully." });
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(
        [FromBody] TransferRequestDto request,
        CancellationToken cancellationToken)
    {
        await _transactionService.TransferAsync(request, cancellationToken);
        return Ok(new { message = "Transfer processed successfully." });
    }
}