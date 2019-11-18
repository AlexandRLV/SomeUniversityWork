using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTOWebApp.Data;
using MTOWebApp.Models;

namespace MTOWebApp.Controllers
{
    public class TestModulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TestModulesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TestModules
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestModule
                .Include(x => x.TheoryModule)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testModule = await _context.TestModule
                .Include(x => x.TheoryModule)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (testModule == null)
            {
                return NotFound();
            }

            string userId = _userManager.GetUserId(User);

            var testScore = await _context.TestScore
                .Include(x => x.TestModule)
                .Where(x => x.TestModule.Id == testModule.Id && x.ApplicationStudent.Id == userId)
                .OrderBy(x => x.TestDate)
                .FirstOrDefaultAsync();

            int qCount = await _context.Question
                .Include(x => x.TestModule)
                .Where(x => x.TestModule.Id == id)
                .CountAsync();

            if (qCount < 9)
                ViewData["AddQuestions"] = "true";
            else
                ViewData["AddQuestions"] = "false";

            if (testScore == null)
                ViewData["LastScore"] = "none";
            else
            {
                ViewData["LastScore"] = testScore.Score;
                ViewData["LastDate"] = testScore.TestDate.ToShortDateString();
            }

            return View(testModule);
        }

        // GET: TestModules/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
                return NotFound();

            var tmodule = _context.TheoryModule
                .SingleOrDefault(x => x.Id == id);

            if (tmodule == null)
                return NotFound();

            TestModule module = new TestModule
            {
                TheoryModule = tmodule
            };

            _context.TestModule.Add(module);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { module.Id });
        }
        
        // GET: TestModules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testModule = await _context.TestModule
                .Include(x => x.TheoryModule)
                .SingleOrDefaultAsync(m => m.Id == id);
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
            var testModule = await _context.TestModule
                .Include(x => x.TheoryModule)
                .SingleOrDefaultAsync(m => m.Id == id);

            var questions = _context.Question
                .Include(x => x.TestModule)
                .Where(x => x.TestModule.Id == testModule.Id);

            var answers = _context.QuestionAnswer
                .Include(x => x.Question)
                .Include(x => x.Question.TestModule)
                .Where(x => x.Question.TestModule.Id == testModule.Id);

            var scores = _context.TestScore
                .Include(x => x.TestModule)
                .Where(x => x.Id == testModule.Id);

            _context.TestModule.Remove(testModule);
            foreach (var q in questions)
                _context.Question.Remove(q);
            foreach (var a in answers)
                _context.Remove(a);
            foreach (var s in scores)
                _context.Remove(s);

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "TheoryModules", new { id = testModule.TheoryModule.Id });
        }

        private bool TestModuleExists(int id)
        {
            return _context.TestModule.Any(e => e.Id == id);
        }
    }
}
