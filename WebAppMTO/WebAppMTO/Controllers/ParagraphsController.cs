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
    public class ParagraphsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParagraphsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Paragraphs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Paragraph.ToListAsync());
        }

        // GET: Paragraphs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paragraph = await _context.Paragraph
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paragraph == null)
            {
                return NotFound();
            }

            return View(paragraph);
        }

        // GET: Paragraphs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Paragraphs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,FolderName,TextFileName,VideoFileName,AnimFileName,SoundFileName")] Paragraph paragraph)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paragraph);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paragraph);
        }

        // GET: Paragraphs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paragraph = await _context.Paragraph.FindAsync(id);
            if (paragraph == null)
            {
                return NotFound();
            }
            return View(paragraph);
        }

        // POST: Paragraphs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FolderName,TextFileName,VideoFileName,AnimFileName,SoundFileName")] Paragraph paragraph)
        {
            if (id != paragraph.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paragraph);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParagraphExists(paragraph.Id))
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
            return View(paragraph);
        }

        // GET: Paragraphs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paragraph = await _context.Paragraph
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paragraph == null)
            {
                return NotFound();
            }

            return View(paragraph);
        }

        // POST: Paragraphs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paragraph = await _context.Paragraph.FindAsync(id);
            _context.Paragraph.Remove(paragraph);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParagraphExists(int id)
        {
            return _context.Paragraph.Any(e => e.Id == id);
        }
    }
}
