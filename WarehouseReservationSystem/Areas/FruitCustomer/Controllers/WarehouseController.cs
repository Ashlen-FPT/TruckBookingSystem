using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseReservationSystem.Data;
using WarehouseReservationSystem.Models.Fruit;

namespace WarehouseReservationSystem.Areas.FruitCustomer.Controllers
{
    [Area("FruitCustomer")]
    public class WarehouseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WarehouseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FruitCustomer/Warehouse
        public async Task<IActionResult> Index()
        {
            return View(await _context.Warehouse.ToListAsync());
        }

        // GET: FruitCustomer/Warehouse/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // GET: FruitCustomer/Warehouse/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FruitCustomer/Warehouse/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NumOfSlots,LocationId")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // GET: FruitCustomer/Warehouse/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouse.FindAsync(id);
            if (warehouse == null)
            {
                return NotFound();
            }
            return View(warehouse);
        }

        // POST: FruitCustomer/Warehouse/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NumOfSlots,LocationId")] Warehouse warehouse)
        {
            if (id != warehouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseExists(warehouse.Id))
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
            return View(warehouse);
        }

        // GET: FruitCustomer/Warehouse/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // POST: FruitCustomer/Warehouse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _context.Warehouse.FindAsync(id);
            _context.Warehouse.Remove(warehouse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarehouseExists(int id)
        {
            return _context.Warehouse.Any(e => e.Id == id);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _context.Warehouse.ToList();
            return Json(new { data = allObj });
        }


        [HttpGet]
        public IActionResult GetWarehouseById(int id)
        {
            var allObj = _context.Warehouse.Where(o => o.Id == id).FirstOrDefault();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _context.Warehouse.Find(id);
            if (objFromDb == null)
            {
                TempData["Error"] = "Error deleting Warehouse";
                return Json(new { success = false, message = "Error while deleting" });
            }

            _context.Warehouse.Remove(objFromDb);
            _context.SaveChanges();

            TempData["Success"] = "Warehouse successfully deleted";
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }
}
