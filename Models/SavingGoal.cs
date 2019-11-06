using Expense_Tracker.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class SavingGoal
    {
        [Key]
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public float Amount { get; set; }
        [Display(Name = "Amount Saved")]
        [CustomRange(0, "Amount", ErrorMessage = "Value cannot be greater than \"Amount\" or less than 0")]
        public float AmountSaved { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Display(Name = "Created")]
        public DateTime CreatedTimestamp { get; set; }

        [NotMapped]
        public int PercentageComplete { get; set; }
    }
}
