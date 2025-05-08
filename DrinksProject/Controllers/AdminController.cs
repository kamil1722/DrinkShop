using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrinksProject.Data;

namespace DrinksProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductsContext _context;

        public AdminController(ProductsContext context)
        {
            _context = context;
        }

        public IActionResult IndexRedirect()
        {
            return RedirectToAction("Index", new { id = 1234 });
        }

        public async Task<IActionResult> Index(int id)
        {
            if (id == 1234)
            {
                var data = await _context.Drinks.ToListAsync();
                return View(data);
            }

            return NotFound();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Count,Price")] Models.Drinks drinks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(drinks);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = 1234 });
            }
            return View(drinks);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Drinks == null)
            {
                return NotFound();
            }

            var drinks = await _context.Drinks.FindAsync(id);
            if (drinks == null)
            {
                return NotFound();
            }
            return View(drinks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Count,Price")] Models.Drinks drinks)
        {
            if (id != drinks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drinks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrinksExists(drinks.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = 1234 });
            }
            return View(drinks);
        }

        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int itemId)
        {
            if (_context.Drinks == null)
            {
                return Problem("Entity set 'ProductsContext.Drinks'  is null.");
            }
            var drinks = await _context.Drinks.FindAsync(itemId);
            if (drinks != null)
            {
                _context.Drinks.Remove(drinks);
            }

            await _context.SaveChangesAsync();

            var data = await _context.Drinks.ToListAsync();
            return View(data);
        }

        private bool DrinksExists(int id)
        {
            return _context.Drinks.Any(e => e.Id == id);
        }
    }
}
