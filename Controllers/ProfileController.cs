using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Profile
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            return View(user);
        }

        // GET: Profile/5
        public async Task<IActionResult> ViewUser(Guid? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id.Value.ToString());
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            List<UserConnection> connections = await _context.UserConnections.Where(x => x.User == user).Include(x => x.User2).ToListAsync();

            if (!user.IsPublic)
            {
                if (!connections.Select(x => x.User2).Any(x => x == currentUser))
                {
                    return Forbid();
                }
            }
            
            return View("ViewOther", user);
        }

        public async Task<IActionResult> ChangeVisibility(Guid? id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            user.IsPublic = !user.IsPublic;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
    }
}