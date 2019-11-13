using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminViewerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminViewerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AdminViewer
        public async Task<ActionResult> Index()
        {
            List<AdminViewer> users = await this.GetUserListWithRole("Admin");
            return View(users);
        }

        // GET: AdminViewer/Create
        public async Task<ActionResult> Create()
        {
            List<AdminViewer> users = await this.GetUserListWithRole("User");
            return View(users);
        }

        // GET: AdminViewer/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (ModelState.IsValid)
            {
                if(id == null)
                {
                    return BadRequest();
                }

                ApplicationUser user = await _userManager.FindByIdAsync(id.Value.ToString());
                var userRoles = await _userManager.GetRolesAsync(user);

                var resultRemoveFromRoles = await _userManager.RemoveFromRolesAsync(user, userRoles);

                //Remove user records here

                var userConnection = await _context.UserConnections.Where(x => x.User2.Id == user.Id || x.User.Id == user.Id).ToListAsync();

                var savingGoals = await _context.SavingGoals.Where(x => x.User.Id == user.Id).ToListAsync();                             
                
                var sheets = await _context.Sheets.Include(x => x.Records).Where(x => x.User.Id == user.Id).ToListAsync();
                var sheetRecords = new List<SheetRecord>();
                sheets.ForEach(x => sheetRecords.AddRange(x.Records));

                savingGoals.ForEach(x => _context.Entry(x).State = EntityState.Deleted);
                userConnection.ForEach(x => _context.Entry(x).State = EntityState.Deleted);
                sheetRecords.ForEach(x => _context.Entry(x).State = EntityState.Deleted);
                sheets.ForEach(x => _context.Entry(x).State = EntityState.Deleted);

                await _context.SaveChangesAsync();

                var resultRemoveUser = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Create));
        }

        public async Task<ActionResult> Admin(Guid? id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id.Value.ToString());
            await _userManager.AddToRoleAsync(user, "Admin");
            await _userManager.RemoveFromRoleAsync(user, "User");
            return RedirectToAction(nameof(Create));
        }

        public async Task<ActionResult> Remove(Guid? id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id.Value.ToString());
            await _userManager.AddToRoleAsync(user, "User");
            await _userManager.RemoveFromRoleAsync(user, "Admin");
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<AdminViewer>> GetUserListWithRole(string role)
        {
            IdentityRole roleId = await _context.Roles.Where(x => x.Name == role).FirstOrDefaultAsync();
            List<string> adminUserIds = await _context.UserRoles.Where(x => x.RoleId == roleId.Id).Select(x => x.UserId).ToListAsync();
            List<ApplicationUser> adminsList = await _context.Users.Where(x => adminUserIds.Contains(x.Id)).ToListAsync();
            List<AdminViewer> admins = AdminViewer.convertUserList(adminsList);
            return admins;
        }
    }
}