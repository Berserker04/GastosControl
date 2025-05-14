using GastosControl.Domain.Entities;
using GastosControl.Domain.Interfaces;
using GastosControl.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GastosControl.Infrastructure.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExpenseHeader>> GetByUserIdAsync(int userId)
        {
            return await _context.ExpenseHeaders
                .Where(e => e.UserId == userId)
                .Include(e => e.Details)
                .ToListAsync();
        }

        public async Task<ExpenseHeader?> GetByIdAsync(int id)
        {
            return await _context.ExpenseHeaders
                .Include(e => e.Details)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(ExpenseHeader header, List<ExpenseDetail> details)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.ExpenseHeaders.AddAsync(header);
                await _context.SaveChangesAsync();

                foreach (var detail in details)
                {
                    detail.HeaderId = header.Id;
                    await _context.ExpenseDetails.AddAsync(detail);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var header = await _context.ExpenseHeaders.Include(h => h.Details).FirstOrDefaultAsync(h => h.Id == id);
            if (header != null)
            {
                _context.ExpenseDetails.RemoveRange(header.Details);
                _context.ExpenseHeaders.Remove(header);
                await _context.SaveChangesAsync();
            }
        }
    }
}
