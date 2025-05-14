using GastosControl.Domain.Entities;

namespace GastosControl.Application.Interfaces
{
    public interface IExpenseService
    {
        Task<List<ExpenseHeader>> GetByUserIdAsync(int userId);
        Task<ExpenseHeader?> GetByIdAsync(int id);
        Task AddAsync(ExpenseHeader header, List<ExpenseDetail> details);
        Task DeleteAsync(int id);
    }
}
