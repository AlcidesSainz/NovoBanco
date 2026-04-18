public class TransferService
{
    private readonly ITransactionService _transactionService;

    public TransferService(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task Transfer(Guid fromId, Guid toId, decimal amount)
    {
        await _transactionService.Transfer(fromId, toId, amount);
    }
}