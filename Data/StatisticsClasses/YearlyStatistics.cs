using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Data.StatisticsClasses
{
    public class YearlyStatistics
    {
        [Display(Name = "Average Yearly Income")]
        public float AverageYearlyIncome { get; set; }

        [Display(Name = "Average Yearly Expenses")]
        public float AverageYearlyExpenses { get; set; }

        [Display(Name = "Average Yearly Income Not Spent")]
        public float IncomeSpent { get; set; }
    }
}
