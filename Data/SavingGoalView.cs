using Expense_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Data
{
    public class SavingGoalView
    {
        public Guid Id { get; set; }

        public IEnumerable<SavingGoal> Goals { get; set; }

        public ApplicationUser AccessingUser { get; set; }

        public ApplicationUser SavingsGoalUser { get; set; }

        public TypeOfAccess AccessLevel { get; set; }
    }
}
