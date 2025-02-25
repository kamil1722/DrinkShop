using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrinksProject.Data;

namespace DrinksProject.Controllers
{
    public class UserController : Controller
    {
        private readonly MyContext _context;

        public UserController(MyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Drinks.Where(x => x.Count > 0).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCount(int codeDrink)
        {
            if (_context.Drinks == null)
            {
                return Problem("Entity set 'MyContext.Drinks' is null.");
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
            return PartialView("Cards",
                _context.Drinks.ToList().Where(x => x.Count > 0).ToList());
        }
    }
}
