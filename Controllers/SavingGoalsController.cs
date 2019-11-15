using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Identity;

namespace Expense_Tracker.Controllers
{
    public class SavingGoalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SavingGoalsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SavingGoals
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            var savingGoals = await _context.SavingGoals.Where(x => x.User == user).ToListAsync();
            savingGoals.ForEach(x => x.PercentageComplete = (int) (x.AmountSaved / x.Amount * 100));
            return View(new SavingGoalView()
            {
                Id = new Guid(),
                AccessingUser = user,
                SavingsGoalUser = user,
                AccessLevel = TypeOfAccess.Write,
                Goals = savingGoals
            });
        }

        public async Task<IActionResult> IndexConnection(Guid id)
        {
            ApplicationUser savingsUser = await _userManager.FindByIdAsync(id.ToString());
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            UserConnection connection = await _context.UserConnections.Where(x => x.User == savingsUser && x.User2 == currentUser).FirstOrDefaultAsync();
            var savingGoals = await _context.SavingGoals.Where(x => x.User == savingsUser).ToListAsync();
            savingGoals.ForEach(x => x.PercentageComplete = (int) (x.AmountSaved / x.Amount * 100));
            TypeOfAccess accessLevel;
            if (connection != null)
            {
                accessLevel = connection.TypeOfAccess;
            }
            else
            {
                if (!savingsUser.IsPublic)
                {
                    return Forbid();
                }
                else
                {
                    accessLevel = TypeOfAccess.ReadOnly;
                }
            }

            return View("Index", new SavingGoalView()
            {
                AccessingUser = currentUser,
                SavingsGoalUser = savingsUser,
                AccessLevel = accessLevel,
                Goals = savingGoals
            });
        }

        // GET: SavingGoals/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savingGoal = await _context.SavingGoals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (savingGoal == null)
            {
                return NotFound();
            }

            return View(savingGoal);
        }

        // GET: SavingGoals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SavingGoals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Amount,AmountSaved,EndDate")] SavingGoal savingGoal)
        {
            if (ModelState.IsValid)
            {                
                savingGoal.Id = Guid.NewGuid();
                savingGoal.CreatedTimestamp = DateTime.UtcNow;
                savingGoal.User = await _userManager.GetUserAsync(HttpContext.User);
                _context.Add(savingGoal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(savingGoal);
        }

        // GET: SavingGoals/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savingGoal = await _context.SavingGoals.FindAsync(id);
            if (savingGoal == null)
            {
                return NotFound();
            }
            return View(savingGoal);
        }

        // POST: SavingGoals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,Amount,AmountSaved,EndDate")] SavingGoal savingGoal)
        {
            if (id != savingGoal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(savingGoal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SavingGoalExists(savingGoal.Id))
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
            return View(savingGoal);
        }

        // GET: SavingGoals/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savingGoal = await _context.SavingGoals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (savingGoal == null)
            {
                return NotFound();
            }

            return View(savingGoal);
        }

        // POST: SavingGoals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var savingGoal = await _context.SavingGoals.FindAsync(id);
            _context.SavingGoals.Remove(savingGoal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SavingGoalExists(Guid id)
        {
            return _context.SavingGoals.Any(e => e.Id == id);
        }
    }
}
