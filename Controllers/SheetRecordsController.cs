using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CsvHelper;

namespace Expense_Tracker.Controllers
{
    [Authorize]
    public class SheetRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _env;

        public SheetRecordsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // TODO: add an index method here for // GET: SheetRecords/ that will redirect to the sheets page

        // GET: SheetRecords/
        public async Task<IActionResult> Index(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            Sheet sheet = await _context.Sheets.Where(x => x.Id == id.Value).Include(x => x.User).Include(x => x.Records).ThenInclude(x => x.Type).FirstOrDefaultAsync();

            if (user != sheet.User && !sheet.User.IsPublic)
            {
                UserConnection connection = await _context.UserConnections.Where(x => x.User == sheet.User && x.User2 == user).FirstOrDefaultAsync();
                if(connection == null)
                {
                    return Forbid();
                }                
            }

            ICollection<SheetRecord> records = sheet.Records;

            if(records == null)
            {
                records = new List<SheetRecord>();
            }

            ViewData["SheetId"] = sheet.Id;
            return View(records);
        }

        // GET: SheetRecords/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheetRecord = await _context.SheetRecords.Include(x => x.Type).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (sheetRecord == null)
            {
                return NotFound();
            }

            Sheet sheet = await _context.Sheets.FindAsync(sheetRecord.SheetId);
            var isUserSheet = await IsUserSheet(sheet.Id);

            if (!isUserSheet)
            {
                return Forbid();
            }

            return View(sheetRecord);
        }

        // GET: SheetRecords/Create/5
        public async Task<IActionResult> Create(Guid sheetId)
        {
            SheetRecord record = new SheetRecord()
            {
                Date = DateTime.Now,
                SheetId = sheetId
            };

            record = await GetRecordTypes(record);

            return View(record);
        }

        // POST: SheetRecords/Create/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Date,Amount,Notes,SheetId,TypeKey")] SheetRecord sheetRecord)
        {
            if (ModelState.IsValid)
            {
                var isUserSheet = await IsUserSheet(sheetRecord.SheetId);

                if (!isUserSheet)
                {
                    return Forbid();
                }

                sheetRecord.Id = Guid.NewGuid();
                sheetRecord.CreatedTimestamp = DateTime.Now;
                sheetRecord.Type = await _context.RecordTypes.Where(x => x.Id == new Guid(sheetRecord.TypeKey)).FirstOrDefaultAsync();
                var sheet = await _context.Sheets.FindAsync(sheetRecord.SheetId);
                sheet.Records.Add(sheetRecord);
                _context.Entry<Sheet>(sheet).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = sheetRecord.SheetId });
            }
            sheetRecord = await GetRecordTypes(sheetRecord);
            return View(sheetRecord);
        }

        // GET: SheetRecords/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheetRecord = await _context.SheetRecords.Include(x => x.Type).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (sheetRecord == null)
            {
                return NotFound();
            }

            Sheet sheet = await _context.Sheets.Where(x => x.Records.Contains(sheetRecord)).FirstOrDefaultAsync<Sheet>();

            var isUserSheet = await IsUserSheet(sheet.Id);

            if (!isUserSheet)
            {
                return Forbid();
            }

            sheetRecord = await GetRecordTypes(sheetRecord);

            sheetRecord.RecordTypes.Where(x => x.Value == sheetRecord.Type.Id.ToString()).FirstOrDefault().Selected = true;

            ViewData["SheetId"] = sheet.Id;
            return View(sheetRecord);
        }

        // POST: SheetRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Date,Amount,Notes,CreatedTimestamp,SheetId,TypeKey")] SheetRecord sheetRecord)
        {
            if (id != sheetRecord.Id)
            {
                return NotFound();
            }

            var isUserSheet = await IsUserSheet(sheetRecord.SheetId);

            if (!isUserSheet)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    sheetRecord.Type = await _context.RecordTypes.Where(x => x.Id == new Guid(sheetRecord.TypeKey)).FirstOrDefaultAsync();
                    _context.Update(sheetRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SheetRecordExists(sheetRecord.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = sheetRecord.SheetId});
            }
            return View(sheetRecord);
        }

        // GET: SheetRecords/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheetRecord = await _context.SheetRecords.Include(x => x.Type).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (sheetRecord == null)
            {
                return NotFound();
            }

            Sheet sheet = await _context.Sheets.FindAsync(sheetRecord.SheetId);
            var isUserSheet = await IsUserSheet(sheet.Id);

            if (!isUserSheet)
            {
                return Forbid();
            }

            return View(sheetRecord);
        }

        // POST: SheetRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            SheetRecord sheetRecord = await _context.SheetRecords.FindAsync(id);
            var sheet = await _context.Sheets.Include(x => x.Records).Where(x => x.Id == sheetRecord.SheetId).FirstOrDefaultAsync<Sheet>();
            var isUserSheet = await IsUserSheet(sheet.Id);

            if (!isUserSheet)
            {
                return Forbid();
            }

            sheet.Records.Remove(sheetRecord);
            _context.Entry(sheet).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = sheet.Id});
        }

        public async Task<FileResult> CsvDownload(Guid? id)
        {
            if (id == null)
            {
                return null;
            }

            var isUserSheet = await IsUserSheet(id.Value);

            if (!isUserSheet)
            {
                return null;
            }

            var sheet = await _context.Sheets.Include(x => x.Records).ThenInclude(x => x.Type).Where(x => x.Id == id).FirstOrDefaultAsync<Sheet>();
            ICollection<SheetRecord> records = sheet.Records;
            if (records == null)
            {
                records = new List<SheetRecord>();
            }

            ICollection<CsvSheetRecords> csvRecords = new List<CsvSheetRecords>();
            foreach(SheetRecord record in records)
            {
                CsvSheetRecords item = new CsvSheetRecords(record);
                csvRecords.Add(item);
            }
            
            var filePath = _env.ContentRootPath + "\\DataFileDownloads\\";
            var fileName = sheet.User.UserName + " " + sheet.Name + ".csv";
            filePath += fileName;
            var writer = new StreamWriter(filePath);
            var csvWriter = new CsvWriter(writer);
            csvWriter.WriteRecords(csvRecords);
            writer.Flush();
            writer.Close();
            
            var content = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.DeleteOnClose);
            var response = File(content, "application/octet-stream", fileName);
            return response;
        }

        private bool SheetRecordExists(Guid id)
        {
            return _context.SheetRecords.Any(e => e.Id == id);
        }

        private async Task<bool> IsUserSheet(Guid sheetId)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            Sheet sheet = await _context.Sheets.Where(x => x.Id == sheetId && x.User == user).FirstOrDefaultAsync();
            return sheet != null;
        }

        private async Task<SheetRecord> GetRecordTypes(SheetRecord record)
        {
            record.RecordTypes = new List<SelectListItem>();

            await _context.RecordTypes.ForEachAsync(x => record.RecordTypes.Add(new SelectListItem(x.Description, x.Id.ToString())));

            return record;
        }
    }
}
