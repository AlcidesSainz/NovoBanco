namespace NovoBanco.Application.Dtos.Accounts;

public class CreateAccountRequestDto
{
    public Guid CustomerId { get; set; }
    public int AccountType { get; set; }
}