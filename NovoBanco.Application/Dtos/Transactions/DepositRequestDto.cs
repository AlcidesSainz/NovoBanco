namespace NovoBanco.Application.Dtos.Transactions;
/// <summary>
/// Dto de solicitud para realizar un depósito en una cuenta bancaria, 
/// que incluye el ID de la cuenta a la que se realizará el depósito, 
/// el monto a depositar y una referencia opcional para la transacción.
/// </summary>
public class DepositRequestDto
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Reference { get; set; } = string.Empty;
}