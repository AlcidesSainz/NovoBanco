public class Transaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public DateTime Timestamp { get; set; }
    public string Reference { get; set; }
    public string Status { get; set; }
}