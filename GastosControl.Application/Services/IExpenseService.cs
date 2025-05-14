using GastosControl.Application.DTOs;
using GastosControl.Domain.Entities;

namespace GastosControl.Application.Interfaces
{
    public interface IExpenseService
    {
        Task<List<ExpenseHeader>> GetByUserIdAsync(int userId);
        Task<ExpenseHeader?> GetByIdAsync(int id);
        Task AddAsync(ExpenseHeader header, List<ExpenseDetail> details);
        Task DeleteAsync(int id);
        Task<List<BudgetMovementDto>> GetMonthlyBudgetSummaryAsync(int userId, int month, int year);
    }
}
