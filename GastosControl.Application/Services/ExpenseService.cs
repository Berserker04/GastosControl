using GastosControl.Application.Interfaces;
using GastosControl.Domain.Entities;
using GastosControl.Domain.Interfaces;
using System.Reflection.PortableExecutable;

namespace GastosControl.Application.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repository;
        private readonly IExpenseQueries _queries;

        public ExpenseService(IExpenseRepository repository, IExpenseQueries queries)
        {
            _repository = repository;
            _queries = queries;
        }

        public async Task<List<ExpenseHeader>> GetByUserIdAsync(int userId)
            => await _repository.GetByUserIdAsync(userId);

        public async Task<ExpenseHeader?> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task AddAsync(ExpenseHeader header, List<ExpenseDetail> details)
        {
            this.ValidateDuplicate(details);
            await this.ValidateExceededBudget(header, details);

            await _repository.AddAsync(header, details);
        }

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);

        public void ValidateDuplicate(List<ExpenseDetail> details)
        {
            var duplicados = details
                .GroupBy(d => d.ExpenseTypeId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicados.Any())
            {
                throw new InvalidOperationException("No puedes registrar el mismo tipo de gasto más de una vez en los detalles.");
            }
        }
        public async Task ValidateExceededBudget(ExpenseHeader header, List<ExpenseDetail> details)
        {
            var month = header.Date.Month;
            var year = header.Date.Year;
            var userId = header.UserId;

            var alertas = new List<string>();

            foreach (var detalle in details)
            {
                var presupuesto = await _queries.GetUserBudgetAsync(userId, detalle.ExpenseTypeId, month, year);

                if (presupuesto != null)
                {
                    var ejecutado = await _queries.GetExecutedAmountAsync(userId, detalle.ExpenseTypeId, month, year);

                    var totalProyectado = ejecutado + detalle.Amount;

                    if (totalProyectado > presupuesto.Amount)
                    {
                        var exceso = totalProyectado - presupuesto.Amount;
                        alertas.Add($"⚠️ Tipo de gasto '{detalle.ExpenseTypeId}' excede el presupuesto de {presupuesto.Amount:C}. Se excede por {exceso:C}");
                    }
                }
            }

            if (alertas.Any())
            {
                throw new InvalidOperationException(string.Join("\n", alertas));
            }

        }
    }
}
