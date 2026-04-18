public interface ITransactionService
{
    Task Transfer(Guid fromId, Guid toId, decimal amount);
}