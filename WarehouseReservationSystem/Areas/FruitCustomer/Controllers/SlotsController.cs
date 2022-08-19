using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseReservationSystem.Data;
using WarehouseReservationSystem.Models.Fruit;
using WarehouseReservationSystem.Utility;

namespace WarehouseReservationSystem.Areas.FruitCustomer.Controllers
{
    [Area("FruitCustomer")]
    public class SlotsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IEmailSender EmailSender { get; set; }

        public SlotsController(IEmailSender emailSender, ApplicationDbContext context)
        {
            _context = context;
            EmailSender = emailSender;
        }

        // GET: FruitCustomer/Slots
        [Authorize(Roles = SD.Role_Supervisor)]
        public async Task<IActionResult> Index()
        {
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");

            return View();
        }

        // GET: FruitCustomer/Slots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slots = await _context.Slots
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slots == null)
            {
                return NotFound();
            }

            return View(slots);
        }

        // GET: FruitCustomer/Slots/Create
        public IActionResult Create()
        {
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");
            return View();
        }

        // POST: FruitCustomer/Slots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,WarehouseId,StartTime,EndTime")] Slots slots)
        {
            if (ModelState.IsValid)
            {
                _context.Add(slots);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", slots.WarehouseId);
            return View(slots);
        }

        // GET: FruitCustomer/Slots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slots = await _context.Slots.FindAsync(id);
            if (slots == null)
            {
                return NotFound();
            }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", slots.WarehouseId);
            return View(slots);
        }

        // POST: FruitCustomer/Slots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,WarehouseId,StartTime,EndTime")] Slots slots)
        {
            if (id != slots.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(slots);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlotsExists(slots.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", slots.WarehouseId);
            return View(slots);
        }

        // GET: FruitCustomer/Slots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slots = await _context.Slots
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slots == null)
            {
                return NotFound();
            }

            return View(slots);
        }

        // POST: FruitCustomer/Slots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slots = await _context.Slots.FindAsync(id);
            _context.Slots.Remove(slots);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SlotsExists(int id)
        {
            return _context.Slots.Any(e => e.Id == id);
        }



        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(int? id, DateTime? date)
        {
            DateTime oDate = Convert.ToDateTime(date);

            var objFromDb = _context.Slots
            .Include(b => b.Warehouse)
            .Where(u => u.Date == oDate).Where(o => o.WarehouseId == id).ToList();

            return Json(new { data = objFromDb });
        }


        [HttpGet]
        public IActionResult GetAllFuture(int? id)
        {

            var objFromDb = _context.Slots
            .Include(b => b.Warehouse)
            .Where(u => u.Date >= DateTime.Now.Date).Where(o => o.WarehouseId == id).ToList();

            return Json(new { data = objFromDb });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _context.Slots.Find(id);
            if (objFromDb == null)
            {
                TempData["Error"] = "Error deleting Exporter";
                return Json(new { success = false, message = "Error while deleting" });
            }

            _context.Slots.Remove(objFromDb);
            _context.SaveChanges();

            TempData["Success"] = "Slot successfully deleted";
            return Json(new { success = true, message = "Delete Successful" });

        }


        [HttpGet]
        public IActionResult GetClosedSlots(DateTime date, int WarehouseId)
        {
            DateTime oDate = Convert.ToDateTime(date);

            var applicationDbContext = _context.Slots.Include(b => b.Warehouse).
                Where(u => u.Date == oDate).Where(o => o.WarehouseId == WarehouseId).ToList();

            return Json(new { data = applicationDbContext });

        }

        [HttpGet]
        public IActionResult GetBookedSlots(DateTime date, int WarehouseId, int sTime, int eTime)
        {
            DateTime oDate = Convert.ToDateTime(date);


            if (sTime > eTime)
            {
                var applicationDbContext = _context.Booking.Include(b => b.Exporter)
               .Include(b => b.Transporter).Include(b => b.MarketType)
               .Include(b => b.Warehouse)
               .Where(u => u.Date == oDate).Where(o => o.WarehouseId == WarehouseId).Where(s => s.RowIndex >= sTime && s.RowIndex <= 24 || s.RowIndex <= eTime).ToList();

                return Json(new { data = applicationDbContext });

            }

            else
            {
                var applicationDbContext = _context.Booking.Include(b => b.Exporter)
               .Include(b => b.Transporter).Include(b => b.MarketType)
               .Include(b => b.Warehouse)
               .Where(u => u.Date == oDate).Where(o => o.WarehouseId == WarehouseId).Where(s => s.RowIndex >= sTime && s.RowIndex <= eTime).ToList();

                return Json(new { data = applicationDbContext });
            }




        }



        [HttpDelete]
        public IActionResult DeleteEmailSlots(int id)
        {
            var booking = _context.Booking.Include(b => b.Exporter)
               .Include(b => b.Transporter).Include(b => b.MarketType)
               .Include(b => b.Warehouse).FirstOrDefault(u => u.Id == id);

            if (booking == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            var toAddress = booking.Email;
            var subject = "BOOKING CANCELLED! Booking Ref: " + booking.BookingRef;
            var body = "Hi !" + "<br />"
                + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" + "<br />"
                + "The following booking has been cancelled:" + "<br />"
                 + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" + "<br />"
                + "Booking Ref: " + "&nbsp;" + booking.BookingRef + "<br />"
                + "Date: " + "&nbsp;" + booking.Date.ToString("dd/MM/yyyy") + "<br />"
                + "Transporter: " + "&nbsp;" + booking.Transporter.Name + "<br />"
                + "Registration: " + "&nbsp;" + booking.Registration + "<br />"
                + "Trailer Reg: " + "&nbsp;" + booking.TrailerReg + "<br />"
                + "Warehouse: " + "&nbsp;" + booking.Warehouse.Name + "<br />"
                + "Exporter: " + "&nbsp;" + booking.Exporter.Name + "<br />"
                + "Name: " + "&nbsp;" + booking.Name + "<br />"
                + "Market Type: " + "&nbsp;" + booking.MarketType.Name + "<br />"
                + "Phone No. : " + "&nbsp;" + booking.PhoneNumber + "<br />"
                + "Email: " + "&nbsp;" + booking.Email + "<br />"
                + "Time: " + "&nbsp;" + booking.RowIndex + " : 00" + "<br />"
                + "Created On: " + booking.CreatedDateUtc.ToString("dd/MM/yyyy HH:mm:ss") + "<br />"
                + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" + "<br />";


            EmailSender.SendEmailAsync(toAddress, subject, body);


            /*var builder = new StringBuilder();
            builder.AppendLine("\"Id\",\"BookingRef\",\"Date\",\"TransporterId\",\"Registration\",\"TrailerReg\",\"WarehouseId\",\"ExporterId\",\"Name\",\"MarketTypeId\",\"PhoneNumber\",\"Email\",\"RowIndex\",\"Status\",\"CreatedDateUtc\",\"IsLate\",\"IsEarly\",\"TsArr\",\"TsDep\",\"GIn\",\"GOut\",\"TsDur\",\"TsToWhDur\",\"WhDur\",\"TBRN\"");
            builder.AppendLine($"\"{booking.Id}\",\"{booking.BookingRef}\",\"{booking.Date}\",\"{booking.TransporterId}\",\"{booking.Registration}\",\"{booking.TrailerReg}\",\"{booking.WarehouseId}\",\"{booking.ExporterId}\",\"{booking.Name}\",\"{booking.MarketTypeId}\",\"{booking.PhoneNumber}\",\"{booking.Email}\",\"{booking.RowIndex}\",\"{booking.Status}\",\"{booking.CreatedDateUtc}\",\"{booking.IsLate}\",\"{booking.IsEarly}\",\"{booking.TsArr}\",\"{booking.TsDep}\",\"{booking.GIn}\",\"{booking.GOut}\",\"{booking.TsDur}\",\"{booking.TsToWhDur}\",\"{booking.WhDur}\",\"{booking.TBRN}\"");

            var webRoot = _env.WebRootPath;
            var file = System.IO.Path.Combine(webRoot, "CSVFile\\DELETE_" + booking.BookingRef + ".csv");
            System.IO.File.WriteAllText(file, builder.ToString());*/

            _context.Booking.Remove(booking);
            _context.SaveChanges();
            return Json(new { success = true, message = "Delete Successful" });

        }


        #endregion


    }
}
