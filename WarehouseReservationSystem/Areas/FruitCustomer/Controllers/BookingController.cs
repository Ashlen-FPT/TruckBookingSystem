using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WarehouseReservationSystem.Data;
using WarehouseReservationSystem.Models;
using WarehouseReservationSystem.Models.Fruit;
using WarehouseReservationSystem.Utility;

namespace WarehouseReservationSystem.Areas.FruitCustomer.Controllers
{
    [Area("FruitCustomer")]
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        public IEmailSender EmailSender { get; set; }
        private IWebHostEnvironment _env;

        public BookingController(UserManager<IdentityUser> userManager, ApplicationDbContext context, IEmailSender emailSender, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            EmailSender = emailSender;
            _env = env;
        }

        // GET: FruitCustomer/Booking
        public IActionResult Index()
        {
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");

            return View();
        }

        // GET: FruitCustomer/Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Exporter)
                .Include(b => b.MarketType)
                .Include(b => b.Transporter)
                .Include(b => b.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: FruitCustomer/Booking/Create
        public IActionResult Create()
        {


            if (User.IsInRole(SD.Role_Supervisor) && SD.isJustLogin == true)
            {

                SD.isJustLogin = false;
                return RedirectToAction(nameof(Index), "Dash", new { area = "FruitAnalytics" });

            }

            if (User.IsInRole(SD.Role_TruckStop) || User.IsInRole(SD.Role_GateUser))
            {
                return RedirectToAction("StatusUpdate");
            }

            /*string Choice = HttpContext.Request.Query["Choice"];

            SD.Choice = Choice;*/

            ViewData["ExporterId"] = new SelectList(_context.Exporter.Where(a => a.IsActive == true), "Id", "Name");
            ViewData["TransporterId"] = new SelectList(_context.Transporter, "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");
            ViewData["MarketTypeId"] = new SelectList(_context.MarketTypes, "Id", "Name");
            return View();
        }

        // POST: FruitCustomer/Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookingRef,Date,TransporterId,Registration,TrailerReg,WarehouseId,ExporterId,Name,MarketTypeId,PhoneNumber,Email,RowIndex,Status,TsArr,TsDep,GIn,GOut,TBRN,NumOfPlts")] Booking booking)
        {

            if (ModelState.IsValid)
            {
                var seq = _context.Sequence.FirstOrDefault(i => i.Id == 1);
                var seqString = seq.FileSequence.ToString();
                seq.FileSequence += 1;


                _context.Add(booking);
                await _context.SaveChangesAsync();
                int id = booking.Id;

                var bookin = await _context.Booking
                                   .Include(b => b.Exporter)
                                   .Include(b => b.MarketType)
                                   .Include(b => b.Transporter)
                                   .Include(b => b.Warehouse)
                                   .FirstOrDefaultAsync(m => m.Id == id);
                if (bookin != null)
                {
                    SendEmail(id);



                    var builder = new StringBuilder();
                    builder.AppendLine($"T001;{bookin.Warehouse.LocationId};I;H1:{bookin.BookingRef};{bookin.Transporter.KeyCode.Trim()};{bookin.Date.ToString("dd/MM/yyyy")};{TimeSpan.FromHours(bookin.RowIndex)};{bookin.Registration};{bookin.TrailerReg};{bookin.Exporter.KeyCode.Trim()};{bookin.Name};{bookin.MarketType.Name};{bookin.PhoneNumber};{bookin.Email};{bookin.CreatedDateUtc.ToString("dd/MM/yyyy")};{bookin.CreatedDateUtc.ToString("HH:mm:ss")};{bookin.NumOfPlts}");



                    if (seqString.Length == 1)
                    {
                        seqString = "00000" + seqString;
                    }

                    if (seqString.Length == 2)
                    {
                        seqString = "0000" + seqString;
                    }

                    if (seqString.Length == 3)
                    {
                        seqString = "000" + seqString;
                    }

                    if (seqString.Length == 4)
                    {
                        seqString = "00" + seqString;
                    }

                    if (seqString.Length == 5)
                    {
                        seqString = "0" + seqString;
                    }



                    var webRoot = _env.WebRootPath;
                    var file = System.IO.Path.Combine(webRoot, "CSVFile\\TA" + seqString + ".csv");
                    System.IO.File.WriteAllText(file, builder.ToString());



                }



                return RedirectToAction("Details", new { id = id });
            }
            ViewData["ExporterId"] = new SelectList(_context.Exporter, "Id", "Name", booking.ExporterId);
            ViewData["TransporterId"] = new SelectList(_context.Transporter, "Id", "Name", booking.TransporterId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", booking.WarehouseId);
            ViewData["MarketTypeId"] = new SelectList(_context.MarketTypes, "Id", "Name", booking.MarketTypeId);
            return View(booking);
        }

        // GET: FruitCustomer/Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["ExporterId"] = new SelectList(_context.Exporter.Where(a => a.IsActive == true), "Id", "Name");
            ViewData["TransporterId"] = new SelectList(_context.Transporter, "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");
            ViewData["MarketTypeId"] = new SelectList(_context.MarketTypes, "Id", "Name");
            return View(booking);
        }

        // POST: FruitCustomer/Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookingRef,Date,TransporterId,Registration,TrailerReg,WarehouseId,ExporterId,Name,MarketTypeId,PhoneNumber,Email,RowIndex,Status,CreatedDateUtc,IsLate,IsEarly,TsArr,TsDep,GIn,GOut,TsDur,TsToWhDur,WhDur,TBRN,UserTruckStop,UserWarehouse,NumOfPlts")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var seq = _context.Sequence.FirstOrDefault(i => i.Id == 1);
                    var seqString = seq.FileSequence.ToString();
                    seq.FileSequence += 1;

                    _context.Update(booking);
                    await _context.SaveChangesAsync();

                    if (booking != null)
                    {
                        var bookin = await _context.Booking
                                   .Include(b => b.Exporter)
                                   .Include(b => b.MarketType)
                                   .Include(b => b.Transporter)
                                   .Include(b => b.Warehouse)
                                   .FirstOrDefaultAsync(m => m.Id == id);
                        var name = bookin.Transporter.Name;


                        var builder = new StringBuilder();
                        builder.AppendLine($"T001;{booking.Warehouse.LocationId};U;H1:{booking.BookingRef};{booking.Transporter.KeyCode};{booking.Date.ToString("dd/MM/yyyy")};{TimeSpan.FromHours(booking.RowIndex)};{booking.Registration};{booking.TrailerReg};{booking.Exporter.KeyCode};{booking.Name};{booking.MarketType.Name};{booking.PhoneNumber};{booking.Email};{booking.CreatedDateUtc.ToString("dd/MM/yyyy")};{booking.CreatedDateUtc.ToString("HH:mm:ss")};{booking.NumOfPlts}");

                        if (seqString.Length == 1)
                        {
                            seqString = "00000" + seqString;
                        }

                        if (seqString.Length == 2)
                        {
                            seqString = "0000" + seqString;
                        }

                        if (seqString.Length == 3)
                        {
                            seqString = "000" + seqString;
                        }

                        if (seqString.Length == 4)
                        {
                            seqString = "00" + seqString;
                        }

                        if (seqString.Length == 5)
                        {
                            seqString = "0" + seqString;
                        }

                        var webRoot = _env.WebRootPath;
                        var file = System.IO.Path.Combine(webRoot, "CSVFile\\TA" + seqString + ".csv");
                        System.IO.File.WriteAllText(file, builder.ToString());

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewData["ExporterId"] = new SelectList(_context.Exporter.Where(a => a.IsActive == true), "Id", "Name");
            ViewData["TransporterId"] = new SelectList(_context.Transporter, "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");
            ViewData["MarketTypeId"] = new SelectList(_context.MarketTypes, "Id", "Name");
            return View(booking);
        }

        // GET: FruitCustomer/Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Exporter)
                .Include(b => b.MarketType)
                .Include(b => b.Transporter)
                .Include(b => b.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: FruitCustomer/Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }

        public IActionResult StatusUpdate()
        {
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");

            return View();
        }

        public async void SendEmail(int? id)
        {
            var booking = await _context.Booking
                .Include(b => b.Exporter)
                .Include(b => b.MarketType)
                .Include(b => b.Transporter)
                .Include(b => b.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);

            var toAddress = booking.Email;
            var subject = "Booking Ref: " + booking.BookingRef;
            var body = "Hi there " + "!" + "<br />"
                + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" + "<br />"
                + "Here are your booking details:" + "<br />"
                 + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" + "<br />"
                + "Booking Ref: " + "&nbsp;" + booking.BookingRef + "<br />"
                + "Date: " + "&nbsp;" + booking.Date.ToString("dd/MM/yyyy") + "<br />"
                + "Transporter: " + "&nbsp;" + booking.Transporter.Name + "<br />"
                + "Registration: " + "&nbsp;" + booking.Registration + "<br />"
                + "Trailer Reg: " + "&nbsp;" + booking.TrailerReg + "<br />"
                + "Warehouse: " + "&nbsp;" + booking.Warehouse.Name + "<br />"
                + "Exporter: " + "&nbsp;" + booking.Exporter.Name + "<br />"
                + "Number of pallets: " + "&nbsp;" + booking.NumOfPlts + "<br />"
                + "Name: " + "&nbsp;" + booking.Name + "<br />"
                + "Market Type: " + "&nbsp;" + booking.MarketType.Name + "<br />"
                + "Phone No. : " + "&nbsp;" + booking.PhoneNumber + "<br />"
                + "Email: " + "&nbsp;" + booking.Email + "<br />"
                + "Time: " + "&nbsp;" + booking.RowIndex + " : 00" + "<br />"
                + "Created On: " + booking.CreatedDateUtc.ToString("dd/MM/yyyy HH:mm:ss") + "<br />"
                + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" + "<br />";

            await EmailSender.SendEmailAsync(toAddress, subject, body);



        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetBooked(DateTime date, int WarehouseId)
        {
            DateTime oDate = Convert.ToDateTime(date);

            var applicationDbContext = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse)
                .Where(u => u.Date == oDate).Where(o => o.WarehouseId == WarehouseId).ToList();

            return Json(new { data = applicationDbContext });

        }

        [HttpGet]
        public async Task<IActionResult> GetBooking(int id)
        {
            var booking = await _context.Booking
                .Include(b => b.Exporter)
                .Include(b => b.MarketType)
                .Include(b => b.Transporter)
                .Include(b => b.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);

            return Json(new { data = booking });

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

            var filtered = userList.Where(r => r.Role == SD.Role_Supervisor || r.Role == SD.Role_Controller).ToList();

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
                NumOfSlots = _context.Warehouse.Find(WId).NumOfSlots,

                ClosedSlots = closedSlots

            };



            string stringjson = JsonConvert.SerializeObject(slotsProp);
            return Json(new { data = slotsProp });

        }

        public async Task Send(int? id, string email)
        {

            ApplicationUser applicationUser = (ApplicationUser)await _userManager.GetUserAsync(User);
            string FirstName = applicationUser?.FirstName;

            var booking = await _context.Booking
                .Include(b => b.Exporter)
                .Include(b => b.MarketType)
                .Include(b => b.Transporter)
                .Include(b => b.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);


            var toAddress = email;
            var subject = "Booking Ref: " + booking.BookingRef;
            var body = "Hi " + FirstName + "!" + "<br />"
                + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" + "<br />"
                + "Here are your booking details:" + "<br />"
                 + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" + "<br />"
                + "Booking Ref: " + "&nbsp;" + booking.BookingRef + "<br />"
                + "Date: " + "&nbsp;" + booking.Date.ToString("dd/MM/yyyy") + "<br />"
                + "Transporter: " + "&nbsp;" + booking.Transporter.Name + "<br />"
                + "Registration: " + "&nbsp;" + booking.Registration + "<br />"
                + "Trailer Reg: " + "&nbsp;" + booking.TrailerReg + "<br />"
                + "Number of pallets: " + "&nbsp;" + booking.NumOfPlts + "<br />"
                + "Warehouse: " + "&nbsp;" + booking.Warehouse.Name + "<br />"
                + "Exporter: " + "&nbsp;" + booking.Exporter.Name + "<br />"
                + "Name: " + "&nbsp;" + booking.Name + "<br />"
                + "Market Type: " + "&nbsp;" + booking.MarketType.Name + "<br />"
                + "Phone No. : " + "&nbsp;" + booking.PhoneNumber + "<br />"
                + "Email: " + "&nbsp;" + booking.Email + "<br />"
                + "Time: " + "&nbsp;" + booking.RowIndex + " : 00" + "<br />"
                + "Created On: " + booking.CreatedDateUtc.ToString("dd/MM/yyyy HH:mm:ss") + "<br />"
                + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" + "<br />";




            await EmailSender.SendEmailAsync(toAddress, subject, body);



        }

        public async Task<IActionResult> GetAllByWarehouseDateAsync(int? id, DateTime? date)
        {

            DateTime oDate = Convert.ToDateTime(date);


            ApplicationUser applicationUser = (ApplicationUser)await _userManager.GetUserAsync(User);
            string userEmail = applicationUser?.Email; // will give the user's Email

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Role_Supervisor) || User.IsInRole(SD.Role_TruckStop) || User.IsInRole(SD.Role_GateUser) || User.IsInRole(SD.Role_Controller))
            {


                var applicationDbContext = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse)
                .Where(u => u.Date == oDate).Where(o => o.WarehouseId == id).ToList();

                return Json(new { data = applicationDbContext });
            }
            else
            {


                var applicationDbContext = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse)
                .Where(u => u.Date == oDate).Where(o => o.WarehouseId == id).Where(x => x.Email == userEmail).ToList();

                return Json(new { data = applicationDbContext });
            }

        }

        public async Task ChangeStatus(int id)
        {


            var booking = await _context.Booking
                .Include(b => b.Exporter)
                .Include(b => b.MarketType)
                .Include(b => b.Transporter)
                .Include(b => b.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking != null)
            {

                ApplicationUser applicationUser = (ApplicationUser)await _userManager.GetUserAsync(User);
                string userEmail = applicationUser?.Email;

                if (booking.Status == "BOOKED")
                {
                    booking.Status = "Truckstop Arrival";
                    booking.TsArr = DateTime.Now;

                    var BookingDate = booking.Date;

                    var time = booking.RowIndex;

                    int val = (time * 60);
                    TimeSpan bookedTime = TimeSpan.FromMinutes(val);
                    DateTime currentTime = DateTime.Now;

                    var Tstime = 0;

                    if (time == 1)
                    {
                        Tstime = 23;
                    }
                    else if (time == 2)
                    {
                        Tstime = 23;
                    }
                    else
                    {
                        Tstime = booking.RowIndex - 2;
                    }

                    int tval = (Tstime * 60);
                    TimeSpan TruckStopTime = TimeSpan.FromMinutes(tval);

                   
                    BookingDate = BookingDate.Date + bookedTime;

                    if (DateTime.Compare(BookingDate, DateTime.Now) < 0)
                    {
                        booking.IsLate = true;
                    }

                    else if (TimeSpan.Compare(currentTime.TimeOfDay, TruckStopTime) == -1 && BookingDate.Date == DateTime.Now.Date)
                    {
                        booking.IsEarly = true;
                    }

                    booking.UserTruckStop = userEmail;





                    var seq = _context.Sequence.FirstOrDefault(i => i.Id == 1);
                    var seqString = seq.FileSequence.ToString();
                    seq.FileSequence += 1;

                    if (seq.FileSequence == 999999)
                    {
                        seq.FileSequence = 1;
                    }

                    var builder = new StringBuilder();
                    builder.AppendLine($"T002;{booking.Warehouse.LocationId};U;H1:{booking.BookingRef};{booking.Registration};{booking.TrailerReg};{booking.TsArr.ToString("dd/MM/yyyy")};{booking.TsArr.ToString("HH:mm:ss")};;;;;;;;{booking.NumOfPlts}");

                    if (seqString.Length == 1)
                    {
                        seqString = "00000" + seqString;
                    }

                    if (seqString.Length == 2)
                    {
                        seqString = "0000" + seqString;
                    }

                    if (seqString.Length == 3)
                    {
                        seqString = "000" + seqString;
                    }

                    if (seqString.Length == 4)
                    {
                        seqString = "00" + seqString;
                    }

                    if (seqString.Length == 5)
                    {
                        seqString = "0" + seqString;
                    }

                    var webRoot = _env.WebRootPath;
                    var file = System.IO.Path.Combine(webRoot, "CSVFile\\TA" + seqString + ".csv");
                    System.IO.File.WriteAllText(file, builder.ToString());



                }

                else if (booking.Status == "Truckstop Arrival")
                {
                    booking.Status = "Truckstop Depature";

                    booking.TsDep = DateTime.Now;

                    DateTime TsArrival = booking.TsArr;

                    DateTime TsDeparture = booking.TsDep;

                    TimeSpan span = TsDeparture.Subtract(TsArrival);

                    booking.TsDur = span.ToString(@"hh\:mm\:ss");

                    booking.UserTruckStop = userEmail;


                    _context.Booking.Update(booking);


                    var seq = _context.Sequence.FirstOrDefault(i => i.Id == 1);
                    var seqString = seq.FileSequence.ToString();
                    seq.FileSequence += 1;

                    if (seq.FileSequence == 999999)
                    {
                        seq.FileSequence = 1;
                    }

                    var builder = new StringBuilder();
                    builder.AppendLine($"T002;{booking.Warehouse.LocationId};U;H1:{booking.BookingRef};{booking.Registration};{booking.TrailerReg};{booking.TsArr.ToString("dd/MM/yyyy")};{booking.TsArr.ToString("HH:mm:ss")};{booking.TsDep.ToString("dd/MM/yyyy")};{booking.TsDep.ToString("HH:mm:ss")};{booking.TBRN};;;;;{booking.NumOfPlts}");
                    if (seqString.Length == 1)
                    {
                        seqString = "00000" + seqString;
                    }

                    if (seqString.Length == 2)
                    {
                        seqString = "0000" + seqString;
                    }

                    if (seqString.Length == 3)
                    {
                        seqString = "000" + seqString;
                    }

                    if (seqString.Length == 4)
                    {
                        seqString = "00" + seqString;
                    }

                    if (seqString.Length == 5)
                    {
                        seqString = "0" + seqString;
                    }
                    var webRoot = _env.WebRootPath;
                    var file = System.IO.Path.Combine(webRoot, "CSVFile\\TA" + seqString + ".csv");
                    System.IO.File.WriteAllText(file, builder.ToString());

                }

                else if (booking.Status == "Truckstop Depature")
                {
                    booking.Status = "Gate In";
                    booking.GIn = DateTime.Now;



                    DateTime GateIn = booking.GIn;

                    DateTime TsDeparture = booking.TsDep;

                    TimeSpan span = GateIn.Subtract(TsDeparture);

                    booking.TsToWhDur = span.ToString(@"hh\:mm\:ss");

                    booking.UserWarehouse = userEmail;



                    var seq = _context.Sequence.FirstOrDefault(i => i.Id == 1);
                    var seqString = seq.FileSequence.ToString();
                    seq.FileSequence += 1;

                    if (seq.FileSequence == 999999)
                    {
                        seq.FileSequence = 1;
                    }

                    var builder = new StringBuilder();
                    builder.AppendLine($"T002;{booking.Warehouse.LocationId};U;H1:{booking.BookingRef};{booking.Registration};{booking.TrailerReg};{booking.TsArr.ToString("dd/MM/yyyy")};{booking.TsArr.ToString("HH:mm:ss")};{booking.TsDep.ToString("dd/MM/yyyy")};{booking.TsDep.ToString("HH:mm:ss")};{booking.TBRN};{booking.GIn.ToString("dd/MM/yyyy")};{booking.GIn.ToString("HH:mm:ss")};;;{booking.NumOfPlts}");

                    if (seqString.Length == 1)
                    {
                        seqString = "00000" + seqString;
                    }

                    if (seqString.Length == 2)
                    {
                        seqString = "0000" + seqString;
                    }

                    if (seqString.Length == 3)
                    {
                        seqString = "000" + seqString;
                    }

                    if (seqString.Length == 4)
                    {
                        seqString = "00" + seqString;
                    }

                    if (seqString.Length == 5)
                    {
                        seqString = "0" + seqString;
                    }

                    var webRoot = _env.WebRootPath;
                    var file = System.IO.Path.Combine(webRoot, "CSVFile\\TA" + seqString + ".csv");
                    System.IO.File.WriteAllText(file, builder.ToString());

                }

                else if (booking.Status == "Gate In")
                {
                    booking.Status = "Gate Out";
                    booking.GOut = DateTime.Now;

                    DateTime GateIn = booking.GIn;

                    DateTime GateOut = booking.GOut;

                    TimeSpan span = GateOut.Subtract(GateIn);

                    booking.WhDur = span.ToString(@"hh\:mm\:ss");

                    booking.UserWarehouse = userEmail;

                    var seq = _context.Sequence.FirstOrDefault(i => i.Id == 1);
                    var seqString = seq.FileSequence.ToString();
                    seq.FileSequence += 1;

                    if (seq.FileSequence == 999999)
                    {
                        seq.FileSequence = 1;
                    }

                    var builder = new StringBuilder();
                    builder.AppendLine($"T002;{booking.Warehouse.LocationId};U;H1:{booking.BookingRef};{booking.Registration};{booking.TrailerReg};{booking.TsArr.ToString("dd/MM/yyyy")};{booking.TsArr.ToString("HH:mm:ss")};{booking.TsDep.ToString("dd/MM/yyyy")};{booking.TsDep.ToString("HH:mm:ss")};{booking.TBRN};{booking.GIn.ToString("dd/MM/yyyy")};{booking.GIn.ToString("HH:mm:ss")};{booking.GOut.ToString("dd/MM/yyyy")};{booking.GOut.ToString("HH:mm:ss")};{booking.NumOfPlts}");

                    if (seqString.Length == 1)
                    {
                        seqString = "00000" + seqString;
                    }

                    if (seqString.Length == 2)
                    {
                        seqString = "0000" + seqString;
                    }

                    if (seqString.Length == 3)
                    {
                        seqString = "000" + seqString;
                    }

                    if (seqString.Length == 4)
                    {
                        seqString = "00" + seqString;
                    }

                    if (seqString.Length == 5)
                    {
                        seqString = "0" + seqString;
                    }

                    var webRoot = _env.WebRootPath;
                    var file = System.IO.Path.Combine(webRoot, "CSVFile\\TA" + seqString + ".csv");
                    System.IO.File.WriteAllText(file, builder.ToString());
                    TimeSpan ElapsedTime = booking.GOut.Subtract(booking.TsArr);
                    booking.ElapsedTime = ElapsedTime.ToString(@"hh\:mm\:ss");
                }

                else if (booking.Status == "Gate Out")
                {
                    booking.Status = "Gate Out";
                }
            }


            _context.Booking.Update(booking);
            _context.SaveChanges();





        }

        public async Task ChangeStatusTBRN(int id, int TBRN)
        {


            var booking = await _context.Booking
                 .Include(b => b.Exporter)
                 .Include(b => b.MarketType)
                 .Include(b => b.Transporter)
                 .Include(b => b.Warehouse)
                 .FirstOrDefaultAsync(m => m.Id == id);

            if (booking != null)
            {
                ApplicationUser applicationUser = (ApplicationUser)await _userManager.GetUserAsync(User);
                string userEmail = applicationUser?.Email;

                if (booking.Status == "BOOKED")
                {
                    booking.Status = "Truckstop Arrival";
                    booking.TsArr = DateTime.Now;



                    var time = booking.RowIndex;

                    int val = (time * 60);
                    TimeSpan bookedTime = TimeSpan.FromMinutes(val);
                    DateTime currentTime = DateTime.Now;

                    var Tstime = 0;

                    if (time == 1)
                    {
                        Tstime = 23;
                    }
                    else if (time == 2)
                    {
                        Tstime = 23;
                    }
                    else
                    {
                        Tstime = booking.RowIndex - 2;
                    }

                    int tval = (Tstime * 60);
                    TimeSpan TruckStopTime = TimeSpan.FromMinutes(tval);


                    if (TimeSpan.Compare(currentTime.TimeOfDay, bookedTime) == 1)
                    {
                        booking.IsLate = true;

                    }

                    else if (TimeSpan.Compare(currentTime.TimeOfDay, TruckStopTime) == -1)
                    {
                        booking.IsEarly = true;
                    }

                    booking.UserTruckStop = userEmail;


                    var seq = _context.Sequence.FirstOrDefault(i => i.Id == 1);
                    var seqString = seq.FileSequence.ToString();
                    seq.FileSequence += 1;

                    if (seq.FileSequence == 999999)
                    {
                        seq.FileSequence = 1;
                    }

                    var builder = new StringBuilder();
                    builder.AppendLine($"T002;{booking.Warehouse.LocationId};U;H1:{booking.BookingRef};{booking.Registration};{booking.TrailerReg};{booking.TsArr.ToString("dd/MM/yyyy")};{booking.TsArr.ToString("HH:mm:ss")};;;;;;;;{booking.NumOfPlts}");

                    if (seqString.Length == 1)
                    {
                        seqString = "00000" + seqString;
                    }

                    if (seqString.Length == 2)
                    {
                        seqString = "0000" + seqString;
                    }

                    if (seqString.Length == 3)
                    {
                        seqString = "000" + seqString;
                    }

                    if (seqString.Length == 4)
                    {
                        seqString = "00" + seqString;
                    }

                    if (seqString.Length == 5)
                    {
                        seqString = "0" + seqString;
                    }

                    var webRoot = _env.WebRootPath;
                    var file = System.IO.Path.Combine(webRoot, "CSVFile\\TA" + seqString + ".csv");
                    System.IO.File.WriteAllText(file, builder.ToString());




                }

                else if (booking.Status == "Truckstop Arrival")
                {
                    booking.Status = "Truckstop Depature";

                    booking.TBRN = TBRN;

                    booking.TsDep = DateTime.Now;

                    DateTime TsArrival = booking.TsArr;

                    DateTime TsDeparture = booking.TsDep;

                    TimeSpan span = TsDeparture.Subtract(TsArrival);

                    booking.TsDur = span.ToString(@"hh\:mm\:ss");
                    booking.UserTruckStop = userEmail;

                    var seq = _context.Sequence.FirstOrDefault(i => i.Id == 1);
                    var seqString = seq.FileSequence.ToString();
                    seq.FileSequence += 1;

                    if (seq.FileSequence == 999999)
                    {
                        seq.FileSequence = 1;
                    }

                    var builder = new StringBuilder();
                    builder.AppendLine($"T002;{booking.Warehouse.LocationId};U;H1:{booking.BookingRef};{booking.Registration};{booking.TrailerReg};{booking.TsArr.ToString("dd/MM/yyyy")};{booking.TsArr.ToString("HH:mm:ss")};{booking.TsDep.ToString("dd/MM/yyyy")};{booking.TsDep.ToString("HH:mm:ss")};{booking.TBRN};;;;;{booking.NumOfPlts}");
                    if (seqString.Length == 1)
                    {
                        seqString = "00000" + seqString;
                    }

                    if (seqString.Length == 2)
                    {
                        seqString = "0000" + seqString;
                    }

                    if (seqString.Length == 3)
                    {
                        seqString = "000" + seqString;
                    }

                    if (seqString.Length == 4)
                    {
                        seqString = "00" + seqString;
                    }

                    if (seqString.Length == 5)
                    {
                        seqString = "0" + seqString;
                    }
                    var webRoot = _env.WebRootPath;
                    var file = System.IO.Path.Combine(webRoot, "CSVFile\\TA" + seqString + ".csv");
                    System.IO.File.WriteAllText(file, builder.ToString());

                }

                else if (booking.Status == "Truckstop Depature")
                {
                    booking.Status = "Gate In";
                    booking.GIn = DateTime.Now;



                    DateTime GateIn = booking.GIn;

                    DateTime TsDeparture = booking.TsDep;

                    TimeSpan span = GateIn.Subtract(TsDeparture);

                    booking.TsToWhDur = span.ToString(@"hh\:mm\:ss");
                    booking.UserWarehouse = userEmail;

                    var seq = _context.Sequence.FirstOrDefault(i => i.Id == 1);
                    var seqString = seq.FileSequence.ToString();
                    seq.FileSequence += 1;

                    if (seq.FileSequence == 999999)
                    {
                        seq.FileSequence = 1;
                    }

                    var builder = new StringBuilder();
                    builder.AppendLine($"T002;{booking.Warehouse.LocationId};U;H1:{booking.BookingRef};{booking.Registration};{booking.TrailerReg};{booking.TsArr.ToString("dd/MM/yyyy")};{booking.TsArr.ToString("HH:mm:ss")};{booking.TsDep.ToString("dd/MM/yyyy")};{booking.TsDep.ToString("HH:mm:ss")};{booking.TBRN};{booking.GIn.ToString("dd/MM/yyyy")};{booking.GIn.ToString("HH:mm:ss")};;;{booking.NumOfPlts}");
                    if (seqString.Length == 1)
                    {
                        seqString = "00000" + seqString;
                    }

                    if (seqString.Length == 2)
                    {
                        seqString = "0000" + seqString;
                    }

                    if (seqString.Length == 3)
                    {
                        seqString = "000" + seqString;
                    }

                    if (seqString.Length == 4)
                    {
                        seqString = "00" + seqString;
                    }

                    if (seqString.Length == 5)
                    {
                        seqString = "0" + seqString;
                    }
                    var webRoot = _env.WebRootPath;
                    var file = System.IO.Path.Combine(webRoot, "CSVFile\\TA" + seqString + ".csv");
                    System.IO.File.WriteAllText(file, builder.ToString());

                }

                else if (booking.Status == "Gate In")
                {
                    booking.Status = "Gate Out";
                    booking.GOut = DateTime.Now;

                    DateTime GateIn = booking.GIn;

                    DateTime GateOut = booking.GOut;

                    TimeSpan span = GateOut.Subtract(GateIn);

                    booking.WhDur = span.ToString(@"hh\:mm\:ss");
                    booking.UserWarehouse = userEmail;


                    var seq = _context.Sequence.FirstOrDefault(i => i.Id == 1);
                    var seqString = seq.FileSequence.ToString();
                    seq.FileSequence += 1;

                    if (seq.FileSequence == 999999)
                    {
                        seq.FileSequence = 1;
                    }

                    var builder = new StringBuilder();
                    builder.AppendLine($"T002;{booking.Warehouse.LocationId};U;H1:{booking.BookingRef};{booking.Registration};{booking.TrailerReg};{booking.TsArr.ToString("dd/MM/yyyy")};{booking.TsArr.ToString("HH:mm:ss")};{booking.TsDep.ToString("dd/MM/yyyy")};{booking.TsDep.ToString("HH:mm:ss")};{booking.TBRN};{booking.GIn.ToString("dd/MM/yyyy")};{booking.GIn.ToString("HH:mm:ss")};{booking.GOut.ToString("dd/MM/yyyy")};{booking.GOut.ToString("HH:mm:ss")};{booking.NumOfPlts}");
                    if (seqString.Length == 1)
                    {
                        seqString = "00000" + seqString;
                    }

                    if (seqString.Length == 2)
                    {
                        seqString = "0000" + seqString;
                    }

                    if (seqString.Length == 3)
                    {
                        seqString = "000" + seqString;
                    }

                    if (seqString.Length == 4)
                    {
                        seqString = "00" + seqString;
                    }

                    if (seqString.Length == 5)
                    {
                        seqString = "0" + seqString;
                    }
                    var webRoot = _env.WebRootPath;
                    var file = System.IO.Path.Combine(webRoot, "CSVFile\\TA" + seqString + ".csv");
                    System.IO.File.WriteAllText(file, builder.ToString());

                }

                else if (booking.Status == "Gate Out")
                {
                    booking.Status = "Gate Out";
                }
            }
            _context.Booking.Update(booking);
            _context.SaveChanges();


        }

        [HttpGet]
        public IActionResult GetBookedForTSG(int id, DateTime date)
        {

            DateTime oDate = Convert.ToDateTime(date);
            if (User.IsInRole(SD.Role_TruckStop))
            {

                var objFromDb = _context.Booking.Include(b => b.Exporter)
                 .Include(b => b.Transporter).Include(b => b.MarketType)
                 .Include(b => b.Warehouse).Where(d => d.Date == oDate).Where(o => o.WarehouseId == id)
                 .Where(s => s.Status == "BOOKED" || s.Status == "Truckstop Arrival").ToList();

                return Json(new { data = objFromDb });
            }
            else if (User.IsInRole(SD.Role_GateUser))
            {
                var objFromDb = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse).Where(d => d.Date == oDate).Where(o => o.WarehouseId == id)
                .Where(s => s.Status == "Truckstop Depature" || s.Status == "Gate In" || s.Status == "Gate Out").ToList();

                return Json(new { data = objFromDb });
            }


            else
            {
                var objFromDb = "";
                return Json(new { data = objFromDb });
            }


        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Booking
                .Include(b => b.Exporter)
                .Include(b => b.MarketType)
                .Include(b => b.Transporter)
                .Include(b => b.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);

            var seq = _context.Sequence.FirstOrDefault(i => i.Id == 1);
            var seqString = seq.FileSequence.ToString();
            seq.FileSequence += 1;

            if (seq.FileSequence == 999999)
            {
                seq.FileSequence = 1;
            }


            var builder = new StringBuilder();
            builder.AppendLine($"T001;{booking.Warehouse.LocationId};D;H1:{booking.BookingRef};{booking.Transporter.KeyCode};{booking.Date.ToString("dd/MM/yyyy")};{TimeSpan.FromHours(booking.RowIndex)};{booking.Registration};{booking.TrailerReg};{booking.Exporter.KeyCode};{booking.Name};{booking.MarketType.Name};{booking.PhoneNumber};{booking.Email};{booking.CreatedDateUtc.ToString("dd/MM/yyyy")};{booking.CreatedDateUtc.ToString("HH:mm:ss")};{booking.NumOfPlts}");
            if (seqString.Length == 1)
            {
                seqString = "00000" + seqString;
            }

            if (seqString.Length == 2)
            {
                seqString = "0000" + seqString;
            }

            if (seqString.Length == 3)
            {
                seqString = "000" + seqString;
            }

            if (seqString.Length == 4)
            {
                seqString = "00" + seqString;
            }

            if (seqString.Length == 5)
            {
                seqString = "0" + seqString;
            }
            var webRoot = _env.WebRootPath;
            var file = System.IO.Path.Combine(webRoot, "CSVFile\\TA" + seqString + ".csv");
            System.IO.File.WriteAllText(file, builder.ToString());
            if (booking == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _context.Booking.Remove(booking);
            _context.SaveChanges();
            return Json(new { success = true, message = "Delete Successful" });

        }


        #endregion
    }

    class SlotsProp
    {
        public int NumOfSlots { get; set; }

        public ArrayList ClosedSlots { get; set; }
    }


    class UserAccess
    {
        public string UserLoggedIn { get; set; }

        public ArrayList AdminUsers { get; set; }




    }
}
