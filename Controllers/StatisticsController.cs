using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Expense_Tracker.Data;
using Expense_Tracker.Data.StatisticsClasses;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        List<Sheet> userSheets;
        List<SheetRecord> userRecords;
        RecordType incomeType;
        string type;
        string typeName;

        RecordType projectedType;
        float projectedIncomeIncrease;
        float projectedExpenseIncrease;
        float projectedRecordIncrease;

        public StatisticsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            userSheets = await _context.Sheets.Where(x => x.User == user).ToListAsync();

            userRecords = await _context.SheetRecords.Where(x => userSheets.Select(a => a.Id).Contains(x.SheetId)).Include(x => x.Type).ToListAsync();

            incomeType = await _context.RecordTypes.Where(x => x.Description.ToLower() == "income").FirstOrDefaultAsync();

            type = "";
            typeName = "";

            StatisticsView view = await this.GenerateViewModel();

            return View(view);
        }

        public async Task<IActionResult> RecordInformation(StatisticsView stats)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            userSheets = await _context.Sheets.Where(x => x.User == user).ToListAsync();

            userRecords = await _context.SheetRecords.Where(x => userSheets.Select(a => a.Id).Contains(x.SheetId)).Include(x => x.Type).ToListAsync();

            incomeType = await _context.RecordTypes.Where(x => x.Description.ToLower() == "income").FirstOrDefaultAsync();

            type = stats.Types;
            typeName = await _context.RecordTypes.Where(x => x.Id.ToString() == type).Select(x => x.Description).FirstOrDefaultAsync();

            stats = await this.GenerateViewModel();

            return View("Index", stats);
        }

        public async Task<IActionResult> NextMonthProjected(StatisticsView stats)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

                userSheets = await _context.Sheets.Where(x => x.User == user).ToListAsync();

                userRecords = await _context.SheetRecords.Where(x => userSheets.Select(a => a.Id).Contains(x.SheetId)).Include(x => x.Type).ToListAsync();

                incomeType = await _context.RecordTypes.Where(x => x.Description.ToLower() == "income").FirstOrDefaultAsync();

                type = "";
                typeName = "";

                projectedRecordIncrease = stats.Projected.RecordIncrease;
                projectedExpenseIncrease = stats.Projected.ExpenseIncrease;
                projectedIncomeIncrease = stats.Projected.IncomeIncrease;
                projectedType = await _context.RecordTypes.FindAsync(new Guid(stats.Projected.RecordType));

                stats = await this.GenerateViewModel();
            }

            return View("Index", stats);
        }

        private async Task<StatisticsView> GenerateViewModel()
        {
            StatisticsView view = new StatisticsView
            {
                MonthlyStats = this.GenerateMonthlyStats(),
                YearlyStats = this.GenerateYearlyStats(),
                RecordTypes = await this.GetRecordTypes(),
                RecordInfoStats = this.GenerateRecordInfoStats(),
                Projected = this.GenerateProjected()
            };            

            return view;
        }

        private NextMonthProjected GenerateProjected()
        {
            NextMonthProjected projected = new NextMonthProjected();
            if(projectedType == null)
            {
                return projected;
            }

            DateTime currentDate = DateTime.UtcNow;

            var totalIncome = userRecords.Where(x => x.Type == incomeType && (x.Date.Month == currentDate.Month || x.Date.Month == currentDate.AddMonths(-1).Month) && !(x.Date.Year == currentDate.Year && x.Date.Month == currentDate.Month)).GroupBy(x => x.Date.Month).ToList().Average(x => x.Average(a => a.Amount));
            var totalExpenses = userRecords.Where(x => x.Type != incomeType && (x.Date.Month == currentDate.Month || x.Date.Month == currentDate.AddMonths(-1).Month) && !(x.Date.Year == currentDate.Year && x.Date.Month == currentDate.Month)).GroupBy(x => x.Date.Month).ToList().Average(x => x.Average(a => a.Amount));
            var expenseIncrease = (100 + projectedExpenseIncrease) / 100;
            var incomeIncrease = (100 + projectedIncomeIncrease) / 100;
            var recordIncreaseNumber = projectedRecordIncrease / 100;
            var recordIncrease = userRecords.Where(x => x.Type == projectedType && (x.Date.Month == currentDate.Month || x.Date.Month == currentDate.AddMonths(-1).Month) && !(x.Date.Year == currentDate.Year && x.Date.Month == currentDate.Month)).GroupBy(x => x.Date.Month).ToList().Average(x => x.Average(a => a.Amount)) * recordIncreaseNumber;
            


            if (projectedType == incomeType)
            {
                totalIncome = (float)Math.Round(totalIncome + recordIncrease, 2);
            }
            else
            {
                totalExpenses = (float)Math.Round(totalExpenses + recordIncrease, 2);
            }

            totalIncome = totalIncome * incomeIncrease;
            totalExpenses = totalExpenses * expenseIncrease;
            var notSpent = (float)Math.Round(totalExpenses / totalIncome * 100, 2);

            projected.ProjectedExpenses = totalExpenses;
            projected.ProjectedIncome = totalIncome;
            projected.ProjectedIncomeSpent = notSpent;
            projected.RecordIncrease = projectedRecordIncrease;

            return projected;
        }

        private MonthlyStatistics GenerateMonthlyStats()
        {
            MonthlyStatistics monthlyStats = new MonthlyStatistics();

            List<float> incomes = new List<float>();
            List<float> expenses = new List<float>();


            userRecords.Where(x => x.Type == incomeType).GroupBy(x => x.Date.Month).ToList().ForEach(x => incomes.Add(x.Sum(a => a.Amount)));
            monthlyStats.AverageIncome = incomes.Average();

            userRecords.Where(x => x.Type != incomeType).GroupBy(x => x.Date.Month).ToList().ForEach(x => expenses.Add(x.Sum(a => a.Amount)));
            monthlyStats.AverageExpenses = expenses.Average();

            monthlyStats.IncomeSpent = (float) Math.Round(monthlyStats.AverageExpenses / monthlyStats.AverageIncome * 100, 2);

            return monthlyStats;
        }

        private YearlyStatistics GenerateYearlyStats()
        {
            YearlyStatistics yearlyStats = new YearlyStatistics();

            List<float> incomes = new List<float>();
            List<float> expenses = new List<float>();


            userRecords.Where(x => x.Type == incomeType).GroupBy(x => x.Date.Year).ToList().ForEach(x => incomes.Add(x.Sum(a => a.Amount)));
            yearlyStats.AverageYearlyIncome = incomes.Average();

            userRecords.Where(x => x.Type != incomeType).GroupBy(x => x.Date.Year).ToList().ForEach(x => expenses.Add(x.Sum(a => a.Amount)));
            yearlyStats.AverageYearlyExpenses = expenses.Average();

            yearlyStats.IncomeSpent = (float) Math.Round(yearlyStats.AverageYearlyExpenses / yearlyStats.AverageYearlyIncome * 100, 2);

            return yearlyStats;
        }

        private RecordInformationStatistics GenerateRecordInfoStats()
        {
            RecordInformationStatistics recordStats = new RecordInformationStatistics()
            {
                MonthAverages = new List<RecordAverage>()
            };

            var monthGroups = userRecords.Where(x => x.Type.Id.ToString() == type).GroupBy(x => x.Date.Month).ToList();

            /*recordStats.DistinctMonths = new List<string>();

            monthGroups.Select(x => x.Key).ToList().ForEach(x => recordStats.DistinctMonths.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x)));*/

            monthGroups.ForEach(x => recordStats.MonthAverages.Add( new RecordAverage()
            {
                Amount = x.Average(a => a.Amount),
                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Key),
                Type = type
            }));

            recordStats.RecordName = typeName;

            return recordStats;
        }

        private async Task<List<SelectListItem>> GetRecordTypes()
        {
            List<SelectListItem> selectItems = new List<SelectListItem>();

            await _context.RecordTypes.ForEachAsync(x => selectItems.Add(new SelectListItem(x.Description, x.Id.ToString())));

            return selectItems;
        }
    }

    public class RecordAndMonth
    {
        public RecordType Record { get; set; }

        public int Month { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            RecordAndMonth value = (RecordAndMonth) obj;

            return (this.Record.Id == value.Record.Id) && (this.Month == value.Month);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Record, Month);
        }
    }
}