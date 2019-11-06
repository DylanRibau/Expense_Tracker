using Expense_Tracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Data
{
    public class CsvSheetRecords
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public float Amount { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedTimestamp { get; set; }

        public CsvSheetRecords()
        {

        }

        public CsvSheetRecords(SheetRecord record)
        {
            Name = record.Name;
            Date = record.Date.ToString("yyyy-MM-dd");
            Amount = record.Amount;
            Type = record.Type.Description;
            Notes = record.Notes;
            CreatedTimestamp = record.CreatedTimestamp;
        }
    }
}
