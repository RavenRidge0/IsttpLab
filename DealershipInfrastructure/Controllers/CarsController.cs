using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DealershipDomain.Model;

namespace DealershipInfrastructure.Controllers
{
    public class CarsController : Controller
    {
        private readonly DealershipContext _context;

        // Конструктор — отримує контекст бази даних через Dependency Injection
        public CarsController(DealershipContext context)
        {
            _context = context;
        }

        // GET: Cars або Cars?id=1&name=Toyota
        // Якщо id не передано — показує всі автомобілі
        // Якщо id передано — показує авто конкретного бренду
        public async Task<IActionResult> Index(int? id, string? name)
        {
            ViewBag.BrandId = id;
            ViewBag.BrandName = name;

            // Якщо бренд не вказано — показуємо всі авто
            var query = _context.Cars.Include(c => c.Brand).AsQueryable();
            if (id != null)
                query = query.Where(c => c.BrandId == id);

            return View(await query.ToListAsync());
        }

        // GET: Cars/Details/5
        // Показує детальну інформацію про один автомобіль
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

        // GET: Cars/Create?id=1 або просто Cars/Create
        // Форма створення нового автомобіля
        public IActionResult Create(int? id)
        {
            ViewBag.BrandId = id;
            if (id.HasValue)
            {
                // Список брендів для випадаючого меню (тільки вибраний бренд)
                ViewData["BrandId"] = new SelectList(
                    _context.Brands.Where(b => b.Id == id), "Id", "Name");
            }
            else
            {
                // Показуємо всі бренди
                ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            }
            return View();
        }

        // POST: Cars/Create
        // Зберігає новий автомобіль у базі даних
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModelName,Year,Price,BrandId")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                // Після збереження — повертаємось до списку авто цього бренду
                var brand = await _context.Brands.FindAsync(car.BrandId);
                return RedirectToAction(nameof(Index), new { id = car.BrandId, name = brand?.Name });
            }
            ViewData["BrandId"] = new SelectList(
                _context.Brands.Where(b => b.Id == car.BrandId), "Id", "Name", car.BrandId);
            return View(car);
        }

        // GET: Cars/Edit/5
        // Форма редагування автомобіля
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound();

            // Список всіх брендів для вибору
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", car.BrandId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // Зберігає зміни автомобіля
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
                // Повертаємось до списку авто бренду
                var brand = await _context.Brands.FindAsync(car.BrandId);
                return RedirectToAction(nameof(Index), new { id = car.BrandId, name = brand?.Name });
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", car.BrandId);
            return View(car);
        }

        // GET: Cars/Delete/5
        // Сторінка підтвердження видалення
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

        // POST: Cars/Delete/5
        // Видаляє автомобіль із бази даних
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
                // Повертаємось до списку авто бренду після видалення
                return RedirectToAction(nameof(Index), new { id = brandId, name = brand?.Name });
            }
            return RedirectToAction(nameof(Index));
        }

        // Допоміжний метод — перевіряє чи існує автомобіль з таким id
        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
