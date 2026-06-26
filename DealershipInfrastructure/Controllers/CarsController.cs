using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DealershipDomain.Model;

namespace DealershipInfrastructure.Controllers
{
    public class CarsController : Controller
    {
        private readonly DealershipContext _context;

        public CarsController(DealershipContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id, string? name)
        {
            ViewBag.BrandId = id;
            ViewBag.BrandName = name;

            var query = _context.Cars.Include(c => c.Brand).AsQueryable();
            if (id != null)
                query = query.Where(c => c.BrandId == id);

            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _context.Cars
                .Include(c => c.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (car == null)
                return NotFound();

            return View(car);
        }

        public IActionResult Create(int? id)
        {
            ViewBag.BrandId = id;
            if (id.HasValue)
            {
                ViewData["BrandId"] = new SelectList(
                    _context.Brands.Where(b => b.Id == id), "Id", "Name");
            }
            else
            {
                ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModelName,Year,Price,BrandId")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                var brand = await _context.Brands.FindAsync(car.BrandId);
                return RedirectToAction(nameof(Index), new { id = car.BrandId, name = brand?.Name });
            }
            ViewData["BrandId"] = new SelectList(
                _context.Brands.Where(b => b.Id == car.BrandId), "Id", "Name", car.BrandId);
            return View(car);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound();

            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", car.BrandId);
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModelName,Year,Price,BrandId")] Car car)
        {
            if (id != car.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                        return NotFound();
                    else
                        throw;
                }
                var brand = await _context.Brands.FindAsync(car.BrandId);
                return RedirectToAction(nameof(Index), new { id = car.BrandId, name = brand?.Name });
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", car.BrandId);
            return View(car);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _context.Cars
                .Include(c => c.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (car == null)
                return NotFound();

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                int brandId = car.BrandId;
                var brand = await _context.Brands.FindAsync(brandId);
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = brandId, name = brand?.Name });
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
