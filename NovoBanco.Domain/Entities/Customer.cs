namespace NovoBanco.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}