public class TransactionService : ITransactionService
{
    private readonly BankingDbContext _context;

    public TransactionService(BankingDbContext context)
    {
        _context = context;
    }

    public async Task Transfer(Guid fromId, Guid toId, decimal amount)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var from = await _context.Accounts.FindAsync(fromId);
            var to = await _context.Accounts.FindAsync(toId);

            if (from == null || to == null)
                throw new Exception("Cuenta no encontrada");

            from.Debit(amount);
            to.Credit(amount);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}