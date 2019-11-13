using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Data.StatisticsClasses
{
    public class MonthlyStatistics
    {
        [Display(Name = "Average Income")]
        public float AverageIncome { get; set; }

        [Display(Name = "Average Expenses")]
        public float AverageExpenses { get; set; }

        [Display(Name = "Average Income Not Spent")]
        public float IncomeSpent { get; set; }
    }
}
