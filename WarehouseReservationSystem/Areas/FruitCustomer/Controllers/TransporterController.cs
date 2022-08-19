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
    public class TransporterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransporterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FruitCustomer/Transporter
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transporter.ToListAsync());
        }

        // GET: FruitCustomer/Transporter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transporter = await _context.Transporter
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transporter == null)
            {
                return NotFound();
            }

            return View(transporter);
        }

        // GET: FruitCustomer/Transporter/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FruitCustomer/Transporter/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KeyCode,Name")] Transporter transporter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transporter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transporter);
        }

        // GET: FruitCustomer/Transporter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transporter = await _context.Transporter.FindAsync(id);
            if (transporter == null)
            {
                return NotFound();
            }
            return View(transporter);
        }

        // POST: FruitCustomer/Transporter/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KeyCode,Name")] Transporter transporter)
        {
            if (id != transporter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transporter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransporterExists(transporter.Id))
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
            return View(transporter);
        }

        // GET: FruitCustomer/Transporter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transporter = await _context.Transporter
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transporter == null)
            {
                return NotFound();
            }

            return View(transporter);
        }

        // POST: FruitCustomer/Transporter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transporter = await _context.Transporter.FindAsync(id);
            _context.Transporter.Remove(transporter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransporterExists(int id)
        {
            return _context.Transporter.Any(e => e.Id == id);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _context.Transporter.ToList();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _context.Transporter.Find(id);
            if (objFromDb == null)
            {
                TempData["Error"] = "Error deleting Transporter";
                return Json(new { success = false, message = "Error while deleting" });
            }


            /* var builder = new StringBuilder();
             builder.AppendLine($"T007;D;H1:{objFromDb.KeyCode};{objFromDb.Name};{objFromDb.Name};Y");

             var webRoot = _env.WebRootPath;
             var file = System.IO.Path.Combine(webRoot, "CSVFile\\DELETE_" + objFromDb.KeyCode + ".csv");
             System.IO.File.WriteAllText(file, builder.ToString());*/

            _context.Transporter.Remove(objFromDb);
            _context.SaveChanges();

            TempData["Success"] = "Transporter successfully deleted";
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion

    }
}
