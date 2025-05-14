using GastosControl.Application.Interfaces;
using GastosControl.Domain.Entities;
using GastosControl.Domain.Interfaces;

namespace GastosControl.Application.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repository;

        public ExpenseService(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ExpenseHeader>> GetByUserIdAsync(int userId)
            => await _repository.GetByUserIdAsync(userId);

        public async Task<ExpenseHeader?> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task AddAsync(ExpenseHeader header, List<ExpenseDetail> details)
            => await _repository.AddAsync(header, details);

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);
    }
}
