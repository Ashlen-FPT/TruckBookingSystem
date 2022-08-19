using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseReservationSystem.Data;
using WarehouseReservationSystem.Utility;

namespace WarehouseReservationSystem.Areas.FruitAnalytics.Controllers
{
    [Area("FruitAnalytics")]
    public class DashController : Controller
    {
        private readonly ApplicationDbContext _context;


        public DashController(ApplicationDbContext context)
        {
            _context = context;

        }

        public ActionResult Index()
        {
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }


        #region API Calls
        public int GetCust()
        {
            var userList = _context.ApplicationUsers.ToList();
            var userRole = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

            }
            var list = userList.Where(l => l.Role == SD.Role_User_Indi);
            return list.Count();
        }
        public IActionResult GetDashBoxes(int? id)
        {
            DateTime now = DateTime.Now.Date;

            var Bookings = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse).Where(i => i.WarehouseId == id).Where(u => u.Date.Month == DateTime.Now.Month).ToList();

            var TodayBookings = Bookings.Where(d => d.Date.Date == DateTime.Now.Date);

            decimal totalAvailable = 0;
            decimal TotalBookings = 0;

            decimal rate = 0;

            if (Bookings.Count() != 0)
            {
                totalAvailable = (Bookings.First().Warehouse.NumOfSlots) * 24;
                TotalBookings = TodayBookings.Count();

                rate = TotalBookings / totalAvailable;


                DashFirstRow dashFirstRow = new DashFirstRow
                {
                    NewBookings = Bookings.Where(d => d.Date.Date > now).Count(),

                    BookingsForToday = Bookings.Where(u => u.Date == now).Count(),

                    Users = GetCust(),

                    Rate = Math.Round(rate, 2) + " %"


                };

                return Json(new { data = dashFirstRow });
            }

            else
            {
                DashFirstRow dashFirstRow = new DashFirstRow
                {
                    NewBookings = 0,

                    BookingsForToday = 0,

                    Users = GetCust(),

                    Rate = "0 %"


                };

                return Json(new { data = dashFirstRow });
            }




        }
        public IActionResult GetPastSixMonths(int? id)
        {
            var applicationDbContext = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse)
                .Where(u => u.Date > DateTime.Now.AddMonths(-7)).Where(i => i.WarehouseId == id).ToList();

            var lastSixMonths = Enumerable.Range(0, 6).Select(i => DateTime.Now.AddMonths(i - 6).ToString("MM/yyyy"));

            Dictionary<string, decimal> monthBookings = new Dictionary<string, decimal>();

            foreach (var monthAndYear in lastSixMonths)
            {
                var temp = Convert.ToDateTime(monthAndYear);

                monthBookings[temp.ToString("MMMM yyyy")] = 0;
            }
            foreach (var monthAndYear in lastSixMonths)
            {
                var temp = Convert.ToDateTime(monthAndYear);

                foreach (var booking in applicationDbContext)
                {
                    if (temp.Month == booking.Date.Month)
                    {
                        if (monthBookings.ContainsKey(temp.ToString("MMMM yyyy")))
                        {
                            monthBookings[temp.ToString("MMMM yyyy")] += 1;
                        }
                        else
                        {
                            monthBookings[temp.ToString("MMMM yyyy")] = 1;

                        }
                    }


                }

            }


            return Json(new { data = monthBookings });

        }
        public IActionResult GetLatePastSixMonths(int? id)
        {
            var applicationDbContext = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse)
                .Where(u => u.Date > DateTime.Now.AddMonths(-7))
                .Where(i => i.WarehouseId == id).Where(l => l.IsLate == true).ToList();

            var lastSixMonths = Enumerable.Range(0, 6).Select(i => DateTime.Now.AddMonths(i - 6).ToString("MM/yyyy"));

            Dictionary<string, decimal> monthBookings = new Dictionary<string, decimal>();

            foreach (var monthAndYear in lastSixMonths)
            {
                var temp = Convert.ToDateTime(monthAndYear);

                monthBookings[temp.ToString("MMMM yyyy")] = 0;
            }
            foreach (var monthAndYear in lastSixMonths)
            {
                var temp = Convert.ToDateTime(monthAndYear);

                foreach (var booking in applicationDbContext)
                {
                    if (temp.Month == booking.Date.Month)
                    {
                        if (monthBookings.ContainsKey(temp.ToString("MMMM yyyy")))
                        {
                            monthBookings[temp.ToString("MMMM yyyy")] += 1;
                        }
                        else
                        {
                            monthBookings[temp.ToString("MMMM yyyy")] = 1;

                        }
                    }


                }

            }


            return Json(new { data = monthBookings });

        }
        public IActionResult GetEarlyPastSixMonths(int? id)
        {
            var applicationDbContext = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse)
                .Where(u => u.Date > DateTime.Now.AddMonths(-7))
                .Where(i => i.WarehouseId == id).Where(e => e.IsEarly == true).ToList();

            var lastSixMonths = Enumerable.Range(0, 6).Select(i => DateTime.Now.AddMonths(i - 6).ToString("MM/yyyy"));

            Dictionary<string, decimal> monthBookings = new Dictionary<string, decimal>();

            foreach (var monthAndYear in lastSixMonths)
            {
                var temp = Convert.ToDateTime(monthAndYear);

                monthBookings[temp.ToString("MMMM yyyy")] = 0;
            }
            foreach (var monthAndYear in lastSixMonths)
            {
                var temp = Convert.ToDateTime(monthAndYear);

                foreach (var booking in applicationDbContext)
                {
                    if (temp.Month == booking.Date.Month)
                    {
                        if (monthBookings.ContainsKey(temp.ToString("MMMM yyyy")))
                        {
                            monthBookings[temp.ToString("MMMM yyyy")] += 1;
                        }
                        else
                        {
                            monthBookings[temp.ToString("MMMM yyyy")] = 1;

                        }
                    }


                }

            }


            return Json(new { data = monthBookings });

        }
        public IActionResult GetPastSixMonthsFilters(int? id)
        {
            var bookings = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse)
                .Where(u => u.Date > DateTime.Now.AddMonths(-7) && u.Date.Month < DateTime.Now.Month)
                .Where(i => i.WarehouseId == id).ToList();


            if (bookings.Count() == 0)
            {
                DashFilters dashFilters = new DashFilters
                {

                    Late = "0 %",
                    Early = "0 %",
                    Unbooked = "0 %",
                    NotArrived = "0 %",
                    Total = "0"
                };

                return Json(new { data = dashFilters });
            }
            else
            {
                var total = bookings.Count();
                var Late = bookings.Where(l => l.IsLate == true).Count();
                var Early = bookings.Where(e => e.IsEarly == true).Count();
                var Unbooked = bookings.Where(a => a.CreatedDateUtc.Date == a.Date.Date).Count();
                var NotArrived = bookings.Where(a => a.CreatedDateUtc.Date != a.Date.Date).Where(s => s.Status == "BOOKED").Count();


                string LatePercent = ((int)Math.Round((double)(100 * Late) / total)).ToString();
                string EarlyPercent = ((int)Math.Round((double)(100 * Early) / total)).ToString();
                string UnbookedPercent = ((int)Math.Round((double)(100 * Unbooked) / total)).ToString();
                string NotArrivedPercent = ((int)Math.Round((double)(100 * NotArrived) / total)).ToString();

                DashFilters dashFilters = new DashFilters
                {

                    Late = LatePercent + "%",
                    Early = EarlyPercent + "%",
                    Unbooked = UnbookedPercent + "%",
                    NotArrived = NotArrivedPercent + "%",
                    Total = total.ToString()
                };

                return Json(new { data = dashFilters });
            }





        }
        public IActionResult GetLastMonthsFilters(int? id)
        {
            var bookings = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse)
                .Where(u => u.Date.Month == DateTime.Now.AddMonths(-1).Month)
                .Where(i => i.WarehouseId == id).ToList();

            var total = bookings.Count();
            var Late = bookings.Where(l => l.IsLate == true).Count();
            var Early = bookings.Where(e => e.IsEarly == true).Count();
            var Unbooked = bookings.Where(a => a.CreatedDateUtc.Date == a.Date.Date).Count();
            var NotArrived = bookings.Where(a => a.CreatedDateUtc.Date != a.Date.Date).Where(s => s.Status == "BOOKED").Count();




            DashFilters dashFilters = new DashFilters
            {

                Late = Late.ToString(),
                Early = Early.ToString(),
                Unbooked = Unbooked.ToString(),
                NotArrived = NotArrived.ToString()


            };

            return Json(new { data = dashFilters });

        }

        public IActionResult GetMarkets(int? id)
        {
            var bookings = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse)
                .Where(u => u.Date.Month == DateTime.Now.AddMonths(-1).Month).Where(i => i.WarehouseId == id).ToList();

            var markets = _context.MarketTypes.ToList();

            var lastSixMonths = Enumerable.Range(0, 6).Select(i => DateTime.Now.AddMonths(i - 6).ToString("MM/yyyy"));



            Dictionary<string, int> monthMarkets = new Dictionary<string, int>();

            foreach (var market in markets)
            {
                var MarketName = market.Name;

                monthMarkets[MarketName] = bookings.Where(m => m.MarketType.Name == MarketName).Count();
            }


            return Json(new { data = monthMarkets });

        }

        public IActionResult GetMarketsThisMonth(int? id)
        {
            var bookings = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse)
                .Where(u => u.Date.Month == DateTime.Now.Month).Where(i => i.WarehouseId == id).ToList();

            var markets = _context.MarketTypes.ToList();

            Dictionary<string, decimal> monthMarkets = new Dictionary<string, decimal>();

            foreach (var market in markets)
            {
                var MarketName = market.Name;

                monthMarkets[MarketName] = bookings.Where(m => m.MarketType.Name == MarketName).Count();
            }


            return Json(new { data = monthMarkets });

        }
        public IActionResult GetBookingToday(int? id)
        {
            DateTime now = DateTime.Now.Date;

            var applicationDbContext = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse)
                .Where(u => u.Date == now).Where(s => s.Status != "Gate Out").Where(i => i.WarehouseId == id).ToList();

            return Json(new { data = applicationDbContext });

        }
        #endregion


    }

    class Markets
    {
        public Dictionary<string, int> MarketInfo { get; set; }
    }

    class DashFilters
    {
        public string Total { get; set; }

        public string Late { get; set; }

        public string Early { get; set; }

        public string Unbooked { get; set; }

        public string NotArrived { get; set; }

    }

    class DashFirstRow
    {

        public int NewBookings { get; set; }

        public string Rate { get; set; }

        public int Users { get; set; }

        public int BookingsForToday { get; set; }


    }

}
