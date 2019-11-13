using Expense_Tracker.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class UserConnection
    {
        [Key]
        public Guid Id { get; set; }
        [Display(Name = "Type of Access")]
        public TypeOfAccess TypeOfAccess { get; set; }
        public ApplicationUser User { get; set; }
        [Display(Name = "User")]
        public ApplicationUser User2 { get; set; }
        public DateTime CreatedTimestamp { get; set; }
    }

    public enum TypeOfAccess
    {
        ReadOnly,
        Write,
        None
    }
}
