using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Data
{
    public class SearchUsers
    {
        public List<ApplicationUser> Users { get; set; }
        [Display(Name = "Search")]
        public string SearchBy { get; set; }
    }
}
