using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GastosControl.Domain.Entities;
using GastosControl.Infrastructure.Persistence;

namespace GastosControl
{
    public class UserBudgetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserBudgetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserBudget
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserBudgets.ToListAsync());
        }

        // GET: UserBudget/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBudget = await _context.UserBudgets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userBudget == null)
            {
                return NotFound();
            }

            return View(userBudget);
        }

        // GET: UserBudget/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserBudget/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ExpenseTypeId,Month,Year,Amount")] UserBudget userBudget)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userBudget);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userBudget);
        }

        // GET: UserBudget/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBudget = await _context.UserBudgets.FindAsync(id);
            if (userBudget == null)
            {
                return NotFound();
            }
            return View(userBudget);
        }

        // POST: UserBudget/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ExpenseTypeId,Month,Year,Amount")] UserBudget userBudget)
        {
            if (id != userBudget.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userBudget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserBudgetExists(userBudget.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userBudget);
        }

        // GET: UserBudget/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBudget = await _context.UserBudgets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userBudget == null)
            {
                return NotFound();
            }

            return View(userBudget);
        }

        // POST: UserBudget/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userBudget = await _context.UserBudgets.FindAsync(id);
            if (userBudget != null)
            {
                _context.UserBudgets.Remove(userBudget);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserBudgetExists(int id)
        {
            return _context.UserBudgets.Any(e => e.Id == id);
        }
    }
}
