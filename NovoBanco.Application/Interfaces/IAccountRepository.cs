public interface IAccountRepository
{
    Task<Account> GetById(Guid id);
    Task Add(Account account);
    Task Update(Account account);
}