using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Data;

namespace Expense_Tracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Expense_Tracker.Models.RecordType> RecordTypes { get; set; }
        public DbSet<Expense_Tracker.Models.SavingGoal> SavingGoals { get; set; }
        public DbSet<Expense_Tracker.Models.Sheet> Sheets { get; set; }
        public DbSet<Expense_Tracker.Models.SheetRecord> SheetRecords { get; set; }
        public DbSet<Expense_Tracker.Models.UserConnection> UserConnections { get; set; }
    }
}
