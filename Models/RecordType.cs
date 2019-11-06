using Expense_Tracker.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class RecordType
    {
        [Key]
        public Guid Id { get; set; }
        public string Description { get; set; }

        [Display(Name = "Created User")]
        public ApplicationUser Createduser { get; set; }
        public DateTime CreatedTimestamp { get; set; }
    }
}
