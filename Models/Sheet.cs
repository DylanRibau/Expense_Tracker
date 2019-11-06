using Expense_Tracker.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class Sheet
    {
        [Key]
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }

        public string Name { get; set; }

        [Range(1,12)]
        public int Month{ get; set; }

        [Range(1700, 2300)]
        public int Year { get; set; }

        public DateTime CreatedTimestamp { get; set; }

        private ICollection<SheetRecord> records;
        public ICollection<SheetRecord> Records
        {
            get
            {
                if(records == null)
                {
                    records = new List<SheetRecord>();
                }
                return records;
            }
            set
            {
                records = value;
            }
        }
    }
}
