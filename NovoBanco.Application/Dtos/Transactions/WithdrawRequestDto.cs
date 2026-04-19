namespace NovoBanco.Application.Dtos.Transactions;
/// <summary>
/// Dto de solicitud para realizar un retiro de una cuenta bancaria
/// </summary>
public class WithdrawRequestDto
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
}