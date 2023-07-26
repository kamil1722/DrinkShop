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
    }
}
