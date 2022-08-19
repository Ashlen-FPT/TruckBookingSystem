using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WarehouseReservationSystem.Models;
using WarehouseReservationSystem.Models.ViewModels;
using WarehouseReservationSystem.Utility;

namespace WarehouseReservationSystem.Controllers
{
    [Area("Selection")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(SD.Role_TruckStop) || User.IsInRole(SD.Role_GateUser))
            {
                return RedirectToAction("StatusUpdate", "Booking", new { area = "FruitCustomer" });
            }

            return View();
        }
        public ActionResult About()
        {


            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
