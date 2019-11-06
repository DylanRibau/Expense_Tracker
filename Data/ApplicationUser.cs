using Expense_Tracker.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Display(Name = "Public Account")]
        public bool IsPublic { get; set; }

        public string SecurityQuestion { get; set; }

        public string SecurityAnswer { get; set; }

        private DateTime lastUpdated;
        [Display(Name = "User Since")]
        public DateTime LastUpdated
        {
            get
            {
                if (lastUpdated == null || lastUpdated == DateTime.MinValue)
                {
                    lastUpdated = DateTime.Now;
                }
                return lastUpdated;
            }
            set
            {
                lastUpdated = value;
            }
        }
        private DateTime createdTimestamp;
        public DateTime CreatedTimestamp
        {
            get
            {
                if (createdTimestamp == null || createdTimestamp == DateTime.MinValue)
                {
                    createdTimestamp = DateTime.Now;
                }
                return createdTimestamp;
            }
            set
            {
                createdTimestamp = value;
            }
        }
    }
}
