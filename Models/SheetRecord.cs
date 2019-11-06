using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class SheetRecord
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public RecordType Type { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public Guid SheetId { get; set; }
        [NotMapped]
        public List<SelectListItem> RecordTypes { get; set; }
        [NotMapped]
        [Required]
        [Display(Name = "Type")]
        public string TypeKey { get; set; }
    }
}
