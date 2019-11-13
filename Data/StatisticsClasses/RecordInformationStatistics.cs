using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Data.StatisticsClasses
{
    public class RecordInformationStatistics
    {
        public List<RecordAverage> MonthAverages { get; set; }

        //public List<string> DistinctMonths { get; set; }

        public string RecordType { get; set; }

        public string RecordName { get; set; }
    }
}
