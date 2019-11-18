﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppMTO.Data;

namespace WebAppMTO.Controllers
{
    public class QuestionAnswersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionAnswersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: QuestionAnswers
        public async Task<IActionResult> Index()
        {
            return View(await _context.QuestionAnswer.ToListAsync());
        }

        // GET: QuestionAnswers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _context.QuestionAnswer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionAnswer == null)
            {
                return NotFound();
            }

            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuestionAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Task,CorrectAnswer,StudentAnswer")] QuestionAnswer questionAnswer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questionAnswer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _context.QuestionAnswer.FindAsync(id);
            if (questionAnswer == null)
            {
                return NotFound();
            }
            return View(questionAnswer);
        }

        // POST: QuestionAnswers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Task,CorrectAnswer,StudentAnswer")] QuestionAnswer questionAnswer)
        {
            if (id != questionAnswer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionAnswer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionAnswerExists(questionAnswer.Id))
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
            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _context.QuestionAnswer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionAnswer == null)
            {
                return NotFound();
            }

            return View(questionAnswer);
        }

        // POST: QuestionAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionAnswer = await _context.QuestionAnswer.FindAsync(id);
            _context.QuestionAnswer.Remove(questionAnswer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionAnswerExists(int id)
        {
            return _context.QuestionAnswer.Any(e => e.Id == id);
        }
    }
}
