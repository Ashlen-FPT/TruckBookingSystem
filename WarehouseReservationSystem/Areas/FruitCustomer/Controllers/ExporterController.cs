using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseReservationSystem.Data;
using WarehouseReservationSystem.Models.Fruit;

namespace WarehouseReservationSystem.Areas.FruitCustomer.Controllers
{
    [Area("FruitCustomer")]
    [Authorize]
    public class ExporterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _env;
        public ExporterController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: FruitCustomer/Exporter
        public async Task<IActionResult> Index()
        {
            return View(await _context.Exporter.ToListAsync());
        }

        public IActionResult ManageIndex()
        {
            return View();
        }

        // GET: FruitCustomer/Exporter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exporter = await _context.Exporter
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exporter == null)
            {
                return NotFound();
            }

            return View(exporter);
        }

        // GET: FruitCustomer/Exporter/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FruitCustomer/Exporter/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KeyCode,Name,Email,EmailActive,IsActive")] Exporter exporter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exporter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exporter);
        }

        // GET: FruitCustomer/Exporter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exporter = await _context.Exporter.FindAsync(id);
            if (exporter == null)
            {
                return NotFound();
            }
            return View(exporter);
        }

        // POST: FruitCustomer/Exporter/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KeyCode,Name,Email,EmailActive,IsActive")] Exporter exporter)
        {
            if (id != exporter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exporter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExporterExists(exporter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ManageIndex));
            }
            return View(exporter);
        }

        // GET: FruitCustomer/Exporter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exporter = await _context.Exporter
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exporter == null)
            {
                return NotFound();
            }

            return View(exporter);
        }

        // POST: FruitCustomer/Exporter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exporter = await _context.Exporter.FindAsync(id);
            _context.Exporter.Remove(exporter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExporterExists(int id)
        {
            return _context.Exporter.Any(e => e.Id == id);
        }



        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _context.Exporter.ToList();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _context.Exporter.Find(id);
            if (objFromDb == null)
            {
                TempData["Error"] = "Error deleting Exporter";
                return Json(new { success = false, message = "Error while deleting" });
            }

            /*var builder = new StringBuilder();



            builder.AppendLine($"T006;D;H1:{objFromDb.KeyCode};;;{objFromDb.Name};{objFromDb.Name};{objFromDb.Email};{objFromDb.IsActive}");

            var webRoot = _env.WebRootPath;
            var file = System.IO.Path.Combine(webRoot, "CSVFile\\DELETE_" + objFromDb.KeyCode + ".csv");
            System.IO.File.WriteAllText(file, builder.ToString());*/

            _context.Exporter.Remove(objFromDb);
            _context.SaveChanges();

            TempData["Success"] = "Exporter successfully deleted";
            return Json(new { success = true, message = "Delete Successful" });

        }

        public IActionResult Deactivate(int? id)
        {

            var emailString = _context.Exporter.Find(id.GetValueOrDefault());
            if (emailString == null)
            {
                return NotFound();
            }

            else
            {
                if (emailString.IsActive == true)
                {
                    emailString.IsActive = false;
                    _context.Exporter.Update(emailString);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Deactivation Successful" });
                }
                else
                {
                    emailString.IsActive = true;
                    _context.Exporter.Update(emailString);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Activation Successful" });
                }
            }

        }


        public IActionResult DeactivateSendUser(int? id)
        {

            var emailString = _context.Exporter.Find(id.GetValueOrDefault());
            if (emailString == null)
            {
                return NotFound();
            }

            else
            {
                if (emailString.EmailActive == true)
                {
                    emailString.EmailActive = false;
                    _context.Exporter.Update(emailString);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Deactivation Successful" });
                }
                else
                {
                    emailString.EmailActive = true;
                    _context.Exporter.Update(emailString);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Activation Successful" });
                }
            }

        }

        #endregion

    }
}
