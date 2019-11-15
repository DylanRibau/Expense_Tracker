using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Globalization;

namespace Expense_Tracker.Controllers
{
    [Authorize]
    public class SheetsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SheetsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Sheets
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            var sheets = _context.Sheets.Where(x => x.User == user).Select(p => p);
            SheetView sheetView = new SheetView()
            {
                Id = new Guid(),
                Sheets = sheets,
                AccessingUser = user,
                SheetsUser = user,
                AccessLevel = TypeOfAccess.Write
            };
            return View(sheetView);
        }

        public async Task<IActionResult> IndexConnection(Guid? id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ApplicationUser sheetUser = await _userManager.FindByIdAsync(id.Value.ToString());
            UserConnection connection = await _context.UserConnections.Where(x => x.User == sheetUser && x.User2 == currentUser).FirstOrDefaultAsync();
            List<Sheet> sheets = await _context.Sheets.Where(x => x.User == sheetUser).Select(p => p).ToListAsync();

            TypeOfAccess accessLevel;
            if(connection != null)
            {
                accessLevel = connection.TypeOfAccess;
            } else
            {
                if (!sheetUser.IsPublic)
                {
                    return Forbid();
                } else
                {
                    accessLevel = TypeOfAccess.ReadOnly;
                }
            }

            SheetView sheetView = new SheetView()
            {
                Id = new Guid(),
                Sheets = sheets,
                SheetsUser = sheetUser,
                AccessingUser = currentUser,
                AccessLevel = accessLevel
            };

            return View("Index", sheetView);
        }

        // GET: Sheets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheet = await _context.Sheets.Where(x => x.Id == id).Include(x => x.User)
                .FirstOrDefaultAsync();
            if (sheet == null)
            {
                return NotFound();
            }

            return View(sheet);
        }

        // GET: Sheets/Create
        public async Task<IActionResult> Create()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            DateTime currentDate = DateTime.UtcNow;
            int monthNumber = currentDate.Month;
            int yearNumber = currentDate.Year;
            string defaultName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(currentDate.Month) + " " + yearNumber;
            Sheet sheet = new Sheet() {
                Month = monthNumber,
                Year = yearNumber,
                Name = defaultName,
                User = user
            };
            return View(sheet);
        }

        // POST: Sheets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Month,Year")] Sheet sheet)
        {
            if (ModelState.IsValid)
            {
                DateTime currentDate = DateTime.UtcNow;
                sheet.Id = Guid.NewGuid();
                sheet.User = await _userManager.GetUserAsync(HttpContext.User);
                sheet.CreatedTimestamp = currentDate;
                _context.Add(sheet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sheet);
        }

        // GET: Sheets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sheet sheet = await _context.Sheets.Where(x => x.Id == id.Value).Include(x => x.User).FirstOrDefaultAsync();

            if (sheet == null)
            {
                return NotFound();
            }

            return View(sheet);
        }

        // POST: Sheets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Month,Year,CreatedTimestamp")] Sheet sheet)
        {
            if (id != sheet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                ApplicationUser sheetUser = await _context.Sheets.Where(x => x.Id == sheet.Id).Include(x => x.User).Select(x => x.User).FirstOrDefaultAsync();
                try
                {                                        
                    if(sheetUser != user)
                    {
                        UserConnection connection = await _context.UserConnections.Where(x => x.User == sheetUser && x.User2 == user).FirstOrDefaultAsync();
                        if(connection == null || connection.TypeOfAccess == TypeOfAccess.ReadOnly)
                        {
                            return Forbid();
                        }
                    }


                    _context.Update(sheet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SheetExists(sheet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if(user == sheetUser)
                {
                    return RedirectToAction(nameof(Index));
                } else
                {
                    return RedirectToAction(nameof(IndexConnection), new { id = new Guid(sheetUser.Id)});
                }                    
            }
            return View(sheet);
        }

        // GET: Sheets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheet = await _context.Sheets.Where(x => x.Id == id.Value).Include(x => x.User)
                .FirstOrDefaultAsync();
            if (sheet == null)
            {
                return NotFound();
            }

            return View(sheet);
        }

        // POST: Sheets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Sheet sheet = await _context.Sheets.FindAsync(id);
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (sheet.User != user)
            {
                UserConnection connection = await _context.UserConnections.Where(x => x.User == sheet.User && x.User2 == user).FirstOrDefaultAsync();
                if (connection.TypeOfAccess == TypeOfAccess.ReadOnly)
                {
                    return Forbid();
                }
            }
            _context.Sheets.Remove(sheet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SheetExists(Guid id)
        {
            return _context.Sheets.Any(e => e.Id == id);
        }
    }
}
