using Expense_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Data
{
    public class SheetView
    {
        public Guid Id { get; set; }

        public IEnumerable<Sheet> Sheets { get; set; }

        public ApplicationUser AccessingUser { get; set; }

        public ApplicationUser SheetsUser { get; set; }

        public TypeOfAccess AccessLevel { get; set; }
    }
}
