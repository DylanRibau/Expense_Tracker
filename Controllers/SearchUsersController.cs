using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Data;
using Microsoft.AspNetCore.Identity;

namespace Expense_Tracker.Controllers
{
    public class SearchUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SearchUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ViewResult Index()
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
            List<ApplicationUser> users = await _context.Users.Where(x => x.UserName.Contains(search.SearchBy)).ToListAsync();
            search.Users = users;
            return View("Index", search);
        }

        private bool SearchUserExists(string id)
        {
            return false;
        }
    }
}
