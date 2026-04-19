public class TransferRequest
{
    public Guid FromId { get; set; }
    public Guid ToId { get; set; }
    public decimal Amount { get; set; }
}