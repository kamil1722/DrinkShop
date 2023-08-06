using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VVM.Data;

namespace VVM.Controllers
{
    public class UserController : Controller
    {
        private readonly VVMContext _context;

        public UserController(VVMContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Drinks.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCount(int codeDrink)
        {
            if (_context.Drinks == null)
            {
                return Problem("Entity set 'VVMContext.Drinks' is null.");
            }
            var drink = await _context.Drinks.FindAsync(codeDrink);
            if (drink != null)
            {
                drink.Count--;
                _context.Drinks.Update(drink);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public int GetPrice(int codeDrink)
        {
            var drinks = _context.Drinks.SingleOrDefault(x => x.Id == codeDrink);

            if (drinks == null)
            {
                return 0;
            }

            return drinks.Price;
        }

        [HttpGet]
        public IActionResult GetCards()
        {
            var cards = _context.Drinks.ToList();
            return PartialView("_Cards", cards);
        }
    }
}
