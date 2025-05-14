using GastosControl.Domain.Entities;
using GastosControl.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class DepositRepository : IDepositRepository
{
    private readonly ApplicationDbContext _context;

    public DepositRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Deposit>> GetByUserIdAsync(int userId)
    {
        return await _context.Deposits
            .Where(d => d.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(Deposit deposit)
    {
        await _context.Deposits.AddAsync(deposit);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var deposit = await _context.Deposits.FindAsync(id);
        if (deposit != null)
        {
            _context.Deposits.Remove(deposit);
            await _context.SaveChangesAsync();
        }
    }
}
