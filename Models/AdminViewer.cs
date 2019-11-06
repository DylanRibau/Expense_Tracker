using Expense_Tracker.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class AdminViewer
    {
        [Key]
        public Guid User { get; set; }

        [Display(Name = "Username")]
        public string Admin { get; set; }    

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        public static List<AdminViewer> convertUserList(List<ApplicationUser> users)
        {
            List<AdminViewer> adminViewers = new List<AdminViewer>();

            users.ForEach(x => adminViewers.Add(new AdminViewer(){
                User = new Guid(x.Id),
                Admin = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName
            }));

            return adminViewers;
        }
    }
}
