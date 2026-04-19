using Microsoft.AspNetCore.Mvc;
using NovoBanco.Application.Dtos.Transactions;
using NovoBanco.Application.Interfaces.Services;

namespace NovoBanco.Api.Controllers;

/// <summary>
/// Controlador para manejo de transacciones bancarias, incluyendo depósitos, retiros y transferencias entre cuentas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// Endpoint para realizar un depósito en una cuenta bancaria.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(
        [FromBody] DepositRequestDto request,
        CancellationToken cancellationToken)
    {
        await _transactionService.DepositAsync(request, cancellationToken);
        return Ok(new { message = "Deposit processed successfully." });
    }

    /// <summary>
    /// Endpoint para realizar un retiro de una cuenta bancaria. 
    /// Se valida que la cuenta tenga saldo suficiente antes de procesar el retiro. 
    /// Si el saldo es insuficiente, se devuelve un error 400 con un mensaje descriptivo. 
    /// Si el retiro es exitoso, se devuelve un código 200 con un mensaje de confirmación.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(
        [FromBody] WithdrawRequestDto request,
        CancellationToken cancellationToken)
    {
        await _transactionService.WithdrawAsync(request, cancellationToken);
        return Ok(new { message = "Withdrawal processed successfully." });
    }

    /// <summary>
    /// Endpoint para realizar una transferencia entre dos cuentas bancarias.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(
        [FromBody] TransferRequestDto request,
        CancellationToken cancellationToken)
    {
        await _transactionService.TransferAsync(request, cancellationToken);
        return Ok(new { message = "Transfer processed successfully." });
    }
}