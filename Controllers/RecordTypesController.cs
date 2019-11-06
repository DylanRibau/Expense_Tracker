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
    public class RecordTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecordTypesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: RecordTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.RecordTypes.Include(x => x.Createduser).ToListAsync());
        }

        // GET: RecordTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recordType = await _context.RecordTypes.Include(x => x.Createduser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recordType == null)
            {
                return NotFound();
            }

            return View(recordType);
        }

        // GET: RecordTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RecordTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] RecordType recordType)
        {
            if (ModelState.IsValid)
            {
                recordType.Id = Guid.NewGuid();
                recordType.CreatedTimestamp = DateTime.UtcNow;
                recordType.Createduser = await _userManager.GetUserAsync(HttpContext.User);
                _context.Add(recordType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recordType);
        }

        // GET: RecordTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recordType = await _context.RecordTypes.FindAsync(id);
            if (recordType == null)
            {
                return NotFound();
            }
            return View(recordType);
        }

        // POST: RecordTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Description,CreatedTimestamp")] RecordType recordType)
        {
            if (id != recordType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    recordType.CreatedTimestamp = DateTime.UtcNow;
                    _context.Update(recordType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordTypeExists(recordType.Id))
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
            return View(recordType);
        }

        // GET: RecordTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recordType = await _context.RecordTypes.Include(x => x.Createduser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recordType == null)
            {
                return NotFound();
            }

            return View(recordType);
        }

        // POST: RecordTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var recordType = await _context.RecordTypes.FindAsync(id);
            _context.RecordTypes.Remove(recordType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordTypeExists(Guid id)
        {
            return _context.RecordTypes.Any(e => e.Id == id);
        }
    }
}
