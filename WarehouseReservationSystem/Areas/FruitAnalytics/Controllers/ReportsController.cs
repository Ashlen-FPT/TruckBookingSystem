using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseReservationSystem.Data;

namespace WarehouseReservationSystem.Areas.FruitAnalytics.Controllers
{
    [Area("FruitAnalytics")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FruitAnalytics/Reports
        public IActionResult Index()
        {
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");

            return View();
        }

        public IActionResult Test()
        {

            return View();
        }


        #region API Calls


        public IActionResult GetRpts(int? id, DateTime StartDate, DateTime EndDate, string filter = null)
        {
            DateTime SDate = Convert.ToDateTime(StartDate);
            DateTime EDate = Convert.ToDateTime(EndDate);



            if (filter == "isLate")
            {
                var cust = _context.Booking.Include(b => b.Exporter)
                 .Include(b => b.Transporter)
                 .Include(b => b.Warehouse).Include(b => b.MarketType)
                 .Where(u => u.Date.Date >= SDate.Date && u.Date.Date <= EDate.Date).Where(i => i.WarehouseId == id).Where(l => l.IsLate == true).ToList();

                return Json(new { data = cust });
            }

            else if (filter == "isEarly")
            {
                var cust = _context.Booking.Include(b => b.Exporter)
                 .Include(b => b.Transporter)
                 .Include(b => b.Warehouse).Include(b => b.MarketType)
                 .Where(u => u.Date.Date >= SDate.Date && u.Date.Date <= EDate.Date).Where(i => i.WarehouseId == id).Where(e => e.IsEarly == true).ToList();

                return Json(new { data = cust });
            }


            else if (filter == "Unbooked")
            {
                var cust = _context.Booking.Include(b => b.Exporter)
                .Include(b => b.Transporter).Include(b => b.MarketType)
                .Include(b => b.Warehouse)
                .Where(u => u.Date.Date >= SDate.Date && u.Date.Date <= EDate.Date)
                .Where(i => i.WarehouseId == id)
                .Where(a => a.CreatedDateUtc.Date == a.Date.Date).ToList();
                return Json(new { data = cust });
            }

            else if (filter == "NotArrived")
            {


                var cust = _context.Booking.Include(b => b.Exporter)
               .Include(b => b.Transporter).Include(b => b.MarketType)
               .Include(b => b.Warehouse)
               .Where(u => u.Date.Date >= SDate.Date && u.Date.Date <= EDate.Date)
               .Where(i => i.WarehouseId == id)
               .Where(a => a.CreatedDateUtc.Date != a.Date.Date).Where(s => s.Status == "BOOKED").ToList();



                return Json(new { data = cust });
            }

            else
            {
                var cust = _context.Booking.Include(b => b.Exporter)
                 .Include(b => b.Transporter)
                 .Include(b => b.Warehouse).Include(b => b.MarketType)
                 .Where(u => u.Date.Date >= SDate.Date && u.Date.Date <= EDate.Date).Where(i => i.WarehouseId == id).ToList();

                return Json(new { data = cust });
            }









        }





        #endregion

    }
}
