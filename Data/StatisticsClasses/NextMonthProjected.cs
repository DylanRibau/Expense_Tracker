using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Data.StatisticsClasses
{
    public class NextMonthProjected
    {
        [Display(Name = "Record Type")]
        public string RecordType { get; set; }

        [Display(Name = "Record Increase Percentage")]
        public float RecordIncrease { get; set; }

        [Display(Name = "Income")]
        public float ProjectedIncome { get; set; }

        [Display(Name = "Expenses")]
        public float ProjectedExpenses { get; set; }

        [Display(Name = "Income Spent")]
        public float ProjectedIncomeSpent { get; set; }

        [Display(Name = "Income Increase Percentage")]
        public float IncomeIncrease { get; set; }

        [Display(Name = "Expense Increase Percentage")]
        public float ExpenseIncrease { get; set; }
    }
}
