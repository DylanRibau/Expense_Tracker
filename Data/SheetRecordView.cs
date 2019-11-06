using Expense_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Data
{
    public class SheetRecordView
    {
        public Guid Id { get; set; }

        public IEnumerable<SheetRecord> Records { get; set; }

        public ApplicationUser AccessingUser { get; set; }

        public ApplicationUser SheetsUser { get; set; }

        public TypeOfAccess AccessLevel { get; set; }
    }
}
