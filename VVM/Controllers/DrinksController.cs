using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VVM.Data;
using VVM.Models;

namespace VVM.Controllers
{
    public class DrinksController : Controller
    {
        private readonly VVMContext _context;

        public DrinksController(VVMContext context)
        {
            _context = context;
        }

        // GET: Drinks
        public async Task<IActionResult> Index()
        {
              return _context.Drinks != null ? 
                          View(await _context.Drinks.ToListAsync()) :
                          Problem("Entity set 'VVMContext.Drinks'  is null.");
        }

        // GET: Drinks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Drinks == null)
            {
                return NotFound();
            }

            var drinks = await _context.Drinks
                .FirstOrDefaultAsync(m => m.id == id);
            if (drinks == null)
            {
                return NotFound();
            }

            return View(drinks);
        }

        // GET: Drinks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,count,price")] Drinks drinks)
        {
            if (ModelState.IsValid)
            {
                drinks.id = Guid.NewGuid();
                _context.Add(drinks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(drinks);
        }

        // GET: Drinks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
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

        // POST: Drinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id,name,description,count,price")] Drinks drinks)
        {
            if (id != drinks.id)
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
                    if (!DrinksExists(drinks.id))
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
            return View(drinks);
        }

        // GET: Drinks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Drinks == null)
            {
                return NotFound();
            }

            var drinks = await _context.Drinks
                .FirstOrDefaultAsync(m => m.id == id);
            if (drinks == null)
            {
                return NotFound();
            }

            return View(drinks);
        }

        // POST: Drinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Drinks == null)
            {
                return Problem("Entity set 'VVMContext.Drinks'  is null.");
            }
            var drinks = await _context.Drinks.FindAsync(id);
            if (drinks != null)
            {
                _context.Drinks.Remove(drinks);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DrinksExists(Guid id)
        {
          return (_context.Drinks?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
