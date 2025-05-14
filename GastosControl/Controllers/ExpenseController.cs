using GastosControl.Application.Interfaces;
using GastosControl.Domain.Entities;
using GastosControl.Helpers;
using GastosControl.Infrastructure.Persistence;
using GastosControl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GastosControl.Controllers
{
    [AuthorizeSession]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly ApplicationDbContext _context;

        public ExpenseController(IExpenseService expenseService, ApplicationDbContext context)
        {
            _expenseService = expenseService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var gastos = await _expenseService.GetByUserIdAsync((int)userId);
            return View(gastos);
        }

        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            ViewBag.MonetaryFundId = new SelectList(_context.MonetaryFunds.Where(f => f.UserId == userId), "Id", "Name");
            ViewBag.ExpenseTypes = new SelectList(_context.ExpenseTypes, "Id", "Name");

            var model = new ExpenseFormViewModel
            {
                Date = DateTime.Today,
                Details = new List<ExpenseDetailInput> { new ExpenseDetailInput() }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseFormViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                ViewBag.MonetaryFundId = new SelectList(_context.MonetaryFunds.Where(f => f.UserId == userId), "Id", "Name");
                ViewBag.ExpenseTypes = new SelectList(_context.ExpenseTypes, "Id", "Name");
                return View(model);
            }

            var header = new ExpenseHeader
            {
                UserId = userId.Value,
                Date = model.Date,
                MonetaryFundId = model.MonetaryFundId,
                CommerceName = model.CommerceName,
                DocumentType = model.DocumentType,
                Observation = model.Observation,
            };

            var details = model.Details.Select(d => new ExpenseDetail
            {
                ExpenseTypeId = d.ExpenseTypeId,
                Amount = d.Amount
            }).ToList();

            await _expenseService.AddAsync(header, details);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var expense = await _expenseService.GetByIdAsync(id);
            if (expense == null || expense.UserId != userId) return NotFound();

            return View(expense);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var expense = await _expenseService.GetByIdAsync(id);
            if (expense == null || expense.UserId != userId) return NotFound();

            return View(expense);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _expenseService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
