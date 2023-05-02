using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PropertyMaintenance.DB;
using PropertyMaintenance.Modles;

namespace PropertyMaintenance.Controllers
{
    public class MaintenancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaintenancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Maintenances
        public async Task<IActionResult> Index()
        {
              return _context.Maintenances != null ? 
                          View(await _context.Maintenances.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Maintenances'  is null.");
        }

        // GET: Maintenances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Maintenances == null)
            {
                return NotFound();
            }

            var maintenance = await _context.Maintenances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        // GET: Maintenances/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Maintenances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Maintenance maintenance)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await file.CopyToAsync(stream);
                            maintenance.Image = stream.ToArray();
                            // Do something with the image data, such as storing it in a database
                        }
                    }
                }
                maintenance.Status = StatusEnum.New;
                maintenance.CreatedDate = DateTime.Now;
                _context.Add(maintenance);

                await _context.SaveChangesAsync();
                TempData["Alert"] = "The new maintenance sucessfully save!";
                return RedirectToAction(nameof(Index));
            }
            return View(maintenance);
        }

        // GET: Maintenances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Maintenances == null)
            {
                return NotFound();
            }

            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance == null)
            {
                return NotFound();
            }
            return View(maintenance);
        }

		// GET: Maintenances/Search/value
		public async Task<IActionResult> Search(string? value)
        {
            if (value == null)
            {
				return RedirectToAction(nameof(Index));
			}

            var maintenance =_context.Maintenances.Where(x => x.EventName.Contains(value) || x.PropertyName.Contains( value ) || x.Description.Contains( value )).ToList();
            if (maintenance == null)
            {
				return RedirectToAction(nameof(Index));
			}
			return View(maintenance);
		}
         
        // POST: Maintenances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Maintenance maintenance)
        {
            if (id != maintenance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count > 0)
                    {
                        foreach (var file in files)
                        {
                            if (file.Length > 0)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    await file.CopyToAsync(stream);
                                    maintenance.Image = stream.ToArray();
                                    // Do something with the image data, such as storing it in a database
                                }
                            }
                        }
                    }
                    else
                    {
                        maintenance.Image = _context.Maintenances.AsNoTracking().FirstOrDefault(x => x.Id == maintenance.Id).Image;
                    }
					_context.Update(maintenance);
                    await _context.SaveChangesAsync();
					TempData["Alert"] = "The maintenance sucessfully edit!!";
				}
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaintenanceExists(maintenance.Id))
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
            return View(maintenance);
        }

        // GET: Maintenances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Maintenances == null)
            {
                return NotFound();
            }

            var maintenance = await _context.Maintenances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        // POST: Maintenances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Maintenances == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Maintenances'  is null.");
            }
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance != null)
            {
                _context.Maintenances.Remove(maintenance);
            }
            
            await _context.SaveChangesAsync();
			TempData["Alert"] = "The maintenance sucessfully Deleted!!";
			return RedirectToAction(nameof(Index));
        }

        private bool MaintenanceExists(int id)
        {
          return (_context.Maintenances?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
