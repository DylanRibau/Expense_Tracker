using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Data.StatisticsClasses
{
    public class StatisticsView
    {
        [Key]
        public Guid Id { get; set; }

        public MonthlyStatistics MonthlyStats { get; set; }

        public YearlyStatistics YearlyStats { get; set; }

        public RecordInformationStatistics RecordInfoStats { get; set; }

        public NextMonthProjected Projected { get; set; }

        public List<SelectListItem> RecordTypes { get; set; }

        public string Types { get; set; }
    }
}
