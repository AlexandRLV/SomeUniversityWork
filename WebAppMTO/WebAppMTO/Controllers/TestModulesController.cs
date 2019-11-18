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
    public class TestModulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TestModules
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestModule.ToListAsync());
        }

        // GET: TestModules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testModule = await _context.TestModule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testModule == null)
            {
                return NotFound();
            }

            return View(testModule);
        }

        // GET: TestModules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] TestModule testModule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testModule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testModule);
        }

        // GET: TestModules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testModule = await _context.TestModule.FindAsync(id);
            if (testModule == null)
            {
                return NotFound();
            }
            return View(testModule);
        }

        // POST: TestModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] TestModule testModule)
        {
            if (id != testModule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testModule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestModuleExists(testModule.Id))
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
            return View(testModule);
        }

        // GET: TestModules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testModule = await _context.TestModule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testModule == null)
            {
                return NotFound();
            }

            return View(testModule);
        }

        // POST: TestModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testModule = await _context.TestModule.FindAsync(id);
            _context.TestModule.Remove(testModule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestModuleExists(int id)
        {
            return _context.TestModule.Any(e => e.Id == id);
        }
    }
}
