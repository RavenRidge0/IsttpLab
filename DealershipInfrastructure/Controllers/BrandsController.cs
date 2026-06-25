using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DealershipDomain.Model;

namespace DealershipInfrastructure.Controllers
{
    public class BrandsController : Controller
    {
        private readonly DealershipContext _context;

        // Конструктор — отримує контекст бази даних через Dependency Injection
        public BrandsController(DealershipContext context)
        {
            _context = context;
        }

        // GET: Brands
        // Показує список усіх брендів разом із кількістю автомобілів кожного
        public async Task<IActionResult> Index()
        {
            // Include(b => b.Cars) — підвантажуємо колекцію авто для кожного бренду
            var brands = await _context.Brands
                .Include(b => b.Cars)
                .ToListAsync();
            return View(brands);
        }

        // GET: Brands/Details/5
        // Замість показу деталей — одразу переходимо до списку авто цього бренду
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return NotFound();

            // Перенаправляємо на сторінку авто конкретного бренду
            return RedirectToAction("Index", "Cars", new { id = brand.Id, name = brand.Name });
        }

        // GET: Brands/Create
        // Порожня форма для створення нового бренду
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // Зберігає новий бренд у базі даних
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Country")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Brands/Edit/5
        // Форма редагування існуючого бренду
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return NotFound();

            return View(brand);
        }

        // POST: Brands/Edit/5
        // Зберігає зміни бренду
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Country")] Brand brand)
        {
            if (id != brand.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Brands/Delete/5
        // Сторінка підтвердження видалення бренду
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var brand = await _context.Brands
                .Include(b => b.Cars)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
                return NotFound();

            return View(brand);
        }

        // POST: Brands/Delete/5
        // Видаляє бренд (і каскадно всі його автомобілі)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
                _context.Brands.Remove(brand);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Допоміжний метод — перевіряє чи існує бренд з таким id
        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.Id == id);
        }
    }
}
