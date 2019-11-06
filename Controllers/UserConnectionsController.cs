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
    public class UserConnectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserConnectionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserConnections
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            var connections = await _context.UserConnections.Where(x => x.User == user).Include(x => x.User2).Include(x => x.User).ToListAsync();
            return View(connections);
        }

        // GET: UserConnections/Create
        public IActionResult Create()
        {
            SearchUsers search = new SearchUsers()
            {
                Users = new List<ApplicationUser>(),
                SearchBy = ""
            };
            return View(search);
        }

        // GET: SearchUsers/2
        public async Task<IActionResult> Search([Bind("SearchBy")] SearchUsers search)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var connections = await _context.UserConnections.Where(x => x.User == currentUser).Include(x => x.User).Include(x => x.User2).ToListAsync();
            List<ApplicationUser> alreadyHasConnection = new List<ApplicationUser>();
            connections.ForEach(x => alreadyHasConnection.Add(x.User2));
            List<ApplicationUser> users = await _context.Users.Where(x => x.UserName.Contains(search.SearchBy) && x != currentUser && !alreadyHasConnection.Contains(x)).ToListAsync();
            search.Users = users;
            return View("Create", search);
        }

        public async Task<IActionResult> AddAccess(string id, TypeOfAccess type)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ApplicationUser user2 = await _userManager.FindByIdAsync(id);
            UserConnection connection = new UserConnection()
            {
                Id = new Guid(),
                TypeOfAccess = type,
                User = currentUser,
                User2 = user2,
                CreatedTimestamp = DateTime.UtcNow

            };
            _context.Add(connection);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create");
        }

        // GET: UserConnections/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            UserConnection connection = await _context.UserConnections.Where(x => x.Id == id.Value).Include(x => x.User).Include(x => x.User2).FirstOrDefaultAsync();

            return View(connection);
        }

        // POST: UserConnections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            UserConnection connection = _context.UserConnections.Where(x => x.Id == id).FirstOrDefault();            
            var userConnection = await _context.UserConnections.FindAsync(id);
            _context.UserConnections.Remove(userConnection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Change(Guid? id)
        {
            if(!id.HasValue)
            {
                return NotFound();
            }

            UserConnection connection = _context.UserConnections.Where(x => x.Id == id.Value).FirstOrDefault();

            if (connection == null)
            {
                return NotFound();
            }

            if(connection.TypeOfAccess == TypeOfAccess.ReadOnly)
            {
                connection.TypeOfAccess = TypeOfAccess.Write;
            } else
            {
                connection.TypeOfAccess = TypeOfAccess.ReadOnly;
            }

            try
            {
                _context.Update(connection);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return RedirectToAction("Index");
        }

        private bool UserConnectionExists(Guid id)
        {
            return _context.UserConnections.Any(e => e.Id == id);
        }
    }
}
