namespace NovoBanco.Application.Dtos.Transactions;
/// <summary>
/// Dto de solicitud para realizar una transferencia entre dos cuentas bancarias
/// </summary>
public class TransferRequestDto
{
    public Guid SourceAccountId { get; set; }
    public Guid DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Reference { get; set; } = string.Empty;
}