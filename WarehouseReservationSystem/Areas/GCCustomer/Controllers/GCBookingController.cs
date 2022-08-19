using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WarehouseReservationSystem.Data;
using WarehouseReservationSystem.Models;
using WarehouseReservationSystem.Models.GC;
using WarehouseReservationSystem.Utility;

namespace WarehouseReservationSystem.Areas.GCCustomer.Controllers
{
    [Area("GCCustomer")]
    public class GCBookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public GCBookingController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: GCCustomer/GCBooking
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GCBooking.Include(g => g.Exporter).Include(g => g.Transporter).Include(g => g.Vessel).Include(g => g.Warehouse);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GCCustomer/GCBooking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gCBooking = await _context.GCBooking
                .Include(g => g.Exporter)
                .Include(g => g.Transporter)
                .Include(g => g.Vessel)
                .Include(g => g.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gCBooking == null)
            {
                return NotFound();
            }

            return View(gCBooking);
        }

        // GET: GCCustomer/GCBooking/Create
        public IActionResult Create()
        {
            ViewData["ExporterId"] = new SelectList(_context.Exporter.Where(a => a.IsActive == true), "Id", "Name");
            ViewData["TransporterId"] = new SelectList(_context.Transporter, "Id", "Name");
            ViewData["VesselId"] = new SelectList(_context.Vessel.Where(a => a.IsActive == true), "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");
            return View();
        }

        // POST: GCCustomer/GCBooking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( GCBooking gCBooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gCBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExporterId"] = new SelectList(_context.Exporter, "Id", "Name", gCBooking.ExporterId);
            ViewData["TransporterId"] = new SelectList(_context.Transporter, "Id", "Name", gCBooking.TransporterId);
            ViewData["VesselId"] = new SelectList(_context.Vessel.Where(a => a.IsActive == true), "Id", "Name", gCBooking.VesselId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", gCBooking.WarehouseId);
            return View(gCBooking);
        }

        public IActionResult Create2()
        {
            ViewData["ExporterId"] = new SelectList(_context.Exporter.Where(a => a.IsActive == true), "Id", "Name");
            ViewData["TransporterId"] = new SelectList(_context.Transporter, "Id", "Name");
            ViewData["VesselId"] = new SelectList(_context.Vessel, "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");
            return View();
        }

        // POST: GCCustomer/GCBooking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2([Bind("Id,BookingRef,Date,WarehouseId,VesselId,ExporterId,TransporterId,Registration,TrailerReg,Import,Export,PhoneNumber,Email,Time,Status,CreatedDateUtc")] GCBooking gCBooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gCBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExporterId"] = new SelectList(_context.Exporter, "Id", "Name", gCBooking.ExporterId);
            ViewData["TransporterId"] = new SelectList(_context.Transporter, "Id", "Name", gCBooking.TransporterId);
            ViewData["VesselId"] = new SelectList(_context.Vessel, "Id", "Name", gCBooking.VesselId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", gCBooking.WarehouseId);
            return View(gCBooking);
        }

        // GET: GCCustomer/GCBooking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gCBooking = await _context.GCBooking.FindAsync(id);
            if (gCBooking == null)
            {
                return NotFound();
            }
            ViewData["ExporterId"] = new SelectList(_context.Exporter, "Id", "KeyCode", gCBooking.ExporterId);
            ViewData["TransporterId"] = new SelectList(_context.Transporter, "Id", "KeyCode", gCBooking.TransporterId);
            ViewData["VesselId"] = new SelectList(_context.Vessel.Where(a => a.IsActive == true), "Id", "Id", gCBooking.VesselId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "LocationId", gCBooking.WarehouseId);
            return View(gCBooking);
        }

        // POST: GCCustomer/GCBooking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookingRef,Date,WarehouseId,VesselId,ExporterId,TransporterId,Registration,TrailerReg,Import,Export,PhoneNumber,Email,Time,Status,CreatedDateUtc")] GCBooking gCBooking)
        {
            if (id != gCBooking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gCBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GCBookingExists(gCBooking.Id))
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
            ViewData["ExporterId"] = new SelectList(_context.Exporter, "Id", "KeyCode", gCBooking.ExporterId);
            ViewData["TransporterId"] = new SelectList(_context.Transporter, "Id", "KeyCode", gCBooking.TransporterId);
            ViewData["VesselId"] = new SelectList(_context.Vessel, "Id", "Id", gCBooking.VesselId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "LocationId", gCBooking.WarehouseId);
            return View(gCBooking);
        }

        // GET: GCCustomer/GCBooking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gCBooking = await _context.GCBooking
                .Include(g => g.Exporter)
                .Include(g => g.Transporter)
                .Include(g => g.Vessel)
                .Include(g => g.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gCBooking == null)
            {
                return NotFound();
            }

            return View(gCBooking);
        }

        // POST: GCCustomer/GCBooking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gCBooking = await _context.GCBooking.FindAsync(id);
            _context.GCBooking.Remove(gCBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GCBookingExists(int id)
        {
            return _context.GCBooking.Any(e => e.Id == id);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAllSlots(DateTime date, int WId)
        {
            DateTime oDate = Convert.ToDateTime(date);
            var closedSlotList = _context.Slots.Where(w => w.WarehouseId == WId).Where(d => d.Date == oDate).ToList();
            int start;
            int end;

            if (closedSlotList.Count() == 0)
            {
                start = 0;
                end = 0;
            }
            else
            {

                var closedSlot = closedSlotList.First();
                start = closedSlot.StartTime;
                end = closedSlot.EndTime;
            }


            var closedSlots = new ArrayList();
            var i = start;
            while (i != end + 1)
            {
                closedSlots.Add(i);

                i += 1;

                if (i == 25 && end <= 12)
                {
                    i = 1;
                }

            }

            SlotsProp slotsProp = new SlotsProp()
            {
                NumOfSlots = _context.Warehouse.Find(WId).GCSlots,

                ClosedSlots = closedSlots

            };



            string stringjson = JsonConvert.SerializeObject(slotsProp);
            return Json(new { data = slotsProp });

        }


        [HttpGet]
        public IActionResult GetBooked(DateTime date, int WarehouseId)
        {
            DateTime oDate = Convert.ToDateTime(date);

            var applicationDbContext = _context.GCBooking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.Vessel)
                .Include(b => b.Warehouse)
                .Where(u => u.Date == oDate).Where(o => o.WarehouseId == WarehouseId).ToList();

            return Json(new { data = applicationDbContext });

        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {

            var userList = _context.ApplicationUsers.ToList();
            var userRole = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

            }

            var filtered = userList.Where(r => r.Role == SD.Role_Supervisor).ToList();

            var list = new ArrayList();

            foreach (var item in filtered)
            {
                list.Add(item.Email);
            }

            ApplicationUser applicationUser = (ApplicationUser)await _userManager.GetUserAsync(User);
            string userEmail = applicationUser?.Email;

            UserAccess userAccess = new UserAccess()
            {

                UserLoggedIn = userEmail,

                AdminUsers = list

            };



            string stringjson = JsonConvert.SerializeObject(userAccess);
            return Json(new { data = userAccess });

        }


        [HttpGet]
        public async Task<IActionResult> GetBooking(int id)
        {
            var booking = await _context.GCBooking
                .Include(b => b.Exporter)
                .Include(b => b.Vessel)
                .Include(b => b.Transporter)
                .Include(b => b.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);

            return Json(new { data = booking });

        }


        #endregion
    }
    class UserAccess
    {
        public string UserLoggedIn { get; set; }

        public ArrayList AdminUsers { get; set; }




    }
    class SlotsProp
    {
        public int NumOfSlots { get; set; }

        public ArrayList ClosedSlots { get; set; }
    }
}
