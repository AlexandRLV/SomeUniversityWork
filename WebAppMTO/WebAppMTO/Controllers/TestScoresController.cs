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
    public class TestScoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestScoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TestScores
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestScore.ToListAsync());
        }

        // GET: TestScores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testScore = await _context.TestScore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testScore == null)
            {
                return NotFound();
            }

            return View(testScore);
        }

        // GET: TestScores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestScores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TestDate,Score")] TestScore testScore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testScore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testScore);
        }

        // GET: TestScores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testScore = await _context.TestScore.FindAsync(id);
            if (testScore == null)
            {
                return NotFound();
            }
            return View(testScore);
        }

        // POST: TestScores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TestDate,Score")] TestScore testScore)
        {
            if (id != testScore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testScore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestScoreExists(testScore.Id))
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
            return View(testScore);
        }

        // GET: TestScores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testScore = await _context.TestScore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testScore == null)
            {
                return NotFound();
            }

            return View(testScore);
        }

        // POST: TestScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testScore = await _context.TestScore.FindAsync(id);
            _context.TestScore.Remove(testScore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestScoreExists(int id)
        {
            return _context.TestScore.Any(e => e.Id == id);
        }
    }
}
