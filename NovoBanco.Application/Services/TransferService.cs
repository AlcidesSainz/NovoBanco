public class TransferService
{
    private readonly IAccountRepository _repo;

    public TransferService(IAccountRepository repo)
    {
        _repo = repo;
    }

    public async Task Transfer(Guid fromId, Guid toId, decimal amount)
    {
        var from = await _repo.GetById(fromId);
        var to = await _repo.GetById(toId);

        from.Debit(amount);
        to.Credit(amount);
    }
}