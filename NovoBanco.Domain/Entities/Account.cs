public class Account
{
    public Guid Id { get; private set; }
    public decimal Balance { get; private set; }
    public AccountStatus Status { get; private set; }

    public byte[] RowVersion { get; set; }

    public void Debit(decimal amount)
    {
        if (Status != AccountStatus.Active)
            throw new Exception("Cuenta no activa");

        if (Balance < amount)
            throw new Exception("Saldo insuficiente");

        Balance -= amount;
    }

    public void Credit(decimal amount)
    {
        if (Status != AccountStatus.Active)
            throw new Exception("Cuenta no activa");

        Balance += amount;
    }
}