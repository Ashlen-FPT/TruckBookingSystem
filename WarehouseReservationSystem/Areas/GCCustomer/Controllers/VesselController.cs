using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseReservationSystem.Data;
using WarehouseReservationSystem.Models.GC;

namespace WarehouseReservationSystem.Areas.GCCustomer.Controllers
{
    [Area("GCCustomer")]
    public class VesselController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VesselController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GCCustomer/Vessel
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vessel.ToListAsync());
        }

        // GET: GCCustomer/Vessel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vessel = await _context.Vessel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vessel == null)
            {
                return NotFound();
            }

            return View(vessel);
        }

        // GET: GCCustomer/Vessel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GCCustomer/Vessel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VesselNo,VoyageNo,Name,IsActive")] Vessel vessel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vessel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vessel);
        }

        // GET: GCCustomer/Vessel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vessel = await _context.Vessel.FindAsync(id);
            if (vessel == null)
            {
                return NotFound();
            }
            return View(vessel);
        }

        // POST: GCCustomer/Vessel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VesselNo,VoyageNo,Name,IsActive")] Vessel vessel)
        {
            if (id != vessel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vessel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VesselExists(vessel.Id))
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
            return View(vessel);
        }

        // GET: GCCustomer/Vessel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vessel = await _context.Vessel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vessel == null)
            {
                return NotFound();
            }

            return View(vessel);
        }

        // POST: GCCustomer/Vessel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vessel = await _context.Vessel.FindAsync(id);
            _context.Vessel.Remove(vessel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VesselExists(int id)
        {
            return _context.Vessel.Any(e => e.Id == id);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {

            var applicationDbContext = _context.Vessel.ToList();

            return Json(new { data = applicationDbContext });

        }


        [HttpGet]

        public IActionResult Deactivate(int? id)
        {

            var vessel = _context.Vessel.Find(id.GetValueOrDefault());
            if (vessel == null)
            {
                return NotFound();
            }

            else
            {
                if (vessel.IsActive == true)
                {
                    vessel.IsActive = false;
                    _context.Vessel.Update(vessel);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Deactivation Successful" });
                }
                else
                {
                    vessel.IsActive = true;
                    _context.Vessel.Update(vessel);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Activation Successful" });
                }
            }

        }


        #endregion

    }
}
