using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppMTO.Data;

namespace WebAppMTO.Controllers
{
    public class TheoryModulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TheoryModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TheoryModules
        public async Task<IActionResult> Index()
        {
            return View(await _context.TheoryModule.ToListAsync());
        }

        // GET: TheoryModules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theoryModule = await _context.TheoryModule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (theoryModule == null)
            {
                return NotFound();
            }

            return View(theoryModule);
        }

        // GET: TheoryModules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TheoryModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] TheoryModule theoryModule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(theoryModule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(theoryModule);
        }

        // GET: TheoryModules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theoryModule = await _context.TheoryModule.FindAsync(id);
            if (theoryModule == null)
            {
                return NotFound();
            }
            return View(theoryModule);
        }

        // POST: TheoryModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] TheoryModule theoryModule)
        {
            if (id != theoryModule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(theoryModule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheoryModuleExists(theoryModule.Id))
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
            return View(theoryModule);
        }

        // GET: TheoryModules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theoryModule = await _context.TheoryModule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (theoryModule == null)
            {
                return NotFound();
            }

            return View(theoryModule);
        }

        // POST: TheoryModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var theoryModule = await _context.TheoryModule.FindAsync(id);
            _context.TheoryModule.Remove(theoryModule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TheoryModuleExists(int id)
        {
            return _context.TheoryModule.Any(e => e.Id == id);
        }
    }
}
