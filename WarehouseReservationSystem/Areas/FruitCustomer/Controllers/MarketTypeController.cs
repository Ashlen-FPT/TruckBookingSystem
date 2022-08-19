using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseReservationSystem.Data;
using WarehouseReservationSystem.Models.Fruit;

namespace WarehouseReservationSystem.Areas.FruitCustomer.Controllers
{
    [Area("FruitCustomer")]
    [Authorize]
    public class MarketTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MarketTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FruitCustomer/MarketType
        public async Task<IActionResult> Index()
        {
            return View(await _context.MarketTypes.ToListAsync());
        }

        // GET: FruitCustomer/MarketType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marketType = await _context.MarketTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marketType == null)
            {
                return NotFound();
            }

            return View(marketType);
        }

        // GET: FruitCustomer/MarketType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FruitCustomer/MarketType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] MarketType marketType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(marketType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(marketType);
        }

        // GET: FruitCustomer/MarketType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marketType = await _context.MarketTypes.FindAsync(id);
            if (marketType == null)
            {
                return NotFound();
            }
            return View(marketType);
        }

        // POST: FruitCustomer/MarketType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] MarketType marketType)
        {
            if (id != marketType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marketType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarketTypeExists(marketType.Id))
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
            return View(marketType);
        }

        // GET: FruitCustomer/MarketType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marketType = await _context.MarketTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marketType == null)
            {
                return NotFound();
            }

            return View(marketType);
        }

        // POST: FruitCustomer/MarketType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var marketType = await _context.MarketTypes.FindAsync(id);
            _context.MarketTypes.Remove(marketType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarketTypeExists(int id)
        {
            return _context.MarketTypes.Any(e => e.Id == id);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _context.MarketTypes.ToList();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _context.MarketTypes.Find(id);
            if (objFromDb == null)
            {
                TempData["Error"] = "Error deleting Market Type";
                return Json(new { success = false, message = "Error while deleting" });
            }

            _context.MarketTypes.Remove(objFromDb);
            _context.SaveChanges();

            TempData["Success"] = "Market Type successfully deleted";
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion

    }
}
