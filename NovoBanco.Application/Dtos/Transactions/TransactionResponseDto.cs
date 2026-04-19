namespace NovoBanco.Application.Dtos.Transactions;
/// <summary>
/// Dto de respuesta para la entidad Transacción, utilizado para devolver 
/// los detalles de una transacción bancaria en las respuestas de la API, 
/// incluyendo información sobre la cuenta de origen, la cuenta de destino
/// , el monto, el tipo de transacción, la referencia, el estado y la fecha de creación.
/// </summary>
public class TransactionResponseDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid? DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
}