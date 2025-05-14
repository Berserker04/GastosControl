using GastosControl.Application.Interfaces;
using GastosControl.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GastosControl.Controllers
{
    public class BudgetController : Controller
    {
        private readonly IExpenseService _expenseService;

        public BudgetController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        public IActionResult Index()
        {
            return View(); // Vista con formulario para seleccionar mes/año
        }

        [HttpPost]
        public async Task<IActionResult> Summary(string BudgetMonth)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var date = BudgetMonth.Split('-');
            var year = int.Parse(date[0]);
            var month = int.Parse(date[1]);

            var resumen = await _expenseService.GetMonthlyBudgetSummaryAsync(userId.Value, month, year);
            ViewBag.Month = month;
            ViewBag.Year = year;
            return View(resumen);
        }
    }

}
