namespace NovoBanco.Application.Dtos.Transactions;

public class TransferRequestDto
{
    public Guid SourceAccountId { get; set; }
    public Guid DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Reference { get; set; } = string.Empty;
}