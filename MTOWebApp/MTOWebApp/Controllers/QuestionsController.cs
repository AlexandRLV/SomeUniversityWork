using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTOWebApp.Data;
using MTOWebApp.Models.ModulesViewModels;
using Microsoft.AspNetCore.Identity;
using MTOWebApp.Models;

namespace MTOWebApp.Controllers
{
    // Контроллер, обрабатывающий запросы к вопросам тестов
    public class QuestionsController : Controller
    {
        // Ссылка на базу данных
        private readonly ApplicationDbContext _context;

        // Менеджер пользователей
        private readonly UserManager<ApplicationUser> _userManager;

        // Конструктор контроллера, применяется иньекция зависимостей
        public QuestionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Отображаем все вопросы, которые содержатся в каком-либо модуле тестирования
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
                return NotFound();

            var testModule = await _context.TestModule
                .Include(x => x.TheoryModule)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (testModule == null)
                return NotFound();

            var questions = await _context.Question
                .Include(x => x.TestModule)
                .Where(x => x.TestModule.Id == id)
                .OrderBy(x => x.SerialNum)
                .ToListAsync();

            ViewData["ModuleName"] = testModule.TheoryModule.Name;
            ViewData["ModuleId"] = testModule.TheoryModule.Id;
            ViewData["TestId"] = testModule.Id;

            return View(questions);
        }

        public async Task<IActionResult> AnswerQuestion(int? score, int? moduleId, int? serialNum)
        {
            if (moduleId == null)
                return NotFound();

            if (!_context.TestModule.Any(x => x.Id == moduleId))
                return NotFound();

            if (serialNum == null)
                serialNum = 1;

            if (score == null)
                score = 0;

            var question = await _context.Question
                .Include(x => x.TestModule)
                .SingleOrDefaultAsync(x => x.TestModule.Id == moduleId && x.SerialNum == serialNum);
            if (question == null)
                return NotFound();

            QuestionViewModel model = new QuestionViewModel
            {
                Id = question.Id,
                Type = question.Type,
                Task = question.Task,
                SerialNum = question.SerialNum,
                TestModuleId = (int)moduleId,
                Answers1 = question.Answers1.Split('&').ToList(),
                Answers2 = question.Answers2 != null ? question.Answers2.Split('&').ToList() : new List<string>()
            };

            ViewData["Score"] = score;

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> AnswerQuestion(int score, QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var question = await _context.Question
                    .Include(x => x.TestModule)
                    .SingleOrDefaultAsync(x => x.Id == model.Id);

                if (question == null)
                    return NotFound();

                if (model.Type != question.Type)
                    return NotFound();

                QuestionAnswer qAnswer = new QuestionAnswer
                {
                    Type = question.Type,
                    Task = question.Task,
                    Answers1 = question.Answers1,
                    Answers2 = question.Answers2,
                    Question = question,
                    Student = await _userManager.GetUserAsync(User)
                };

                if (model.Type == "Input")
                {
                    if (model.Answers1[0] == question.Answers1)
                        score++;
                    qAnswer.StudentAnswer = model.Answers1[0];
                }
                else if (model.Type == "Choose")
                {
                    var answers = question.Answers1.Split('&').ToList();
                    if (answers.Count == model.Answers1.Count)
                    {
                        bool flag = true;
                        for (int i = 0; i < answers.Count; i++)
                        {
                            if (!answers.Contains(model.Answers1[i]))
                                flag = false;
                        }

                        if (flag)
                            score++;
                    }

                    string s = "";
                    for (int i = 0; i < model.Answers1.Count; i++)
                    {
                        s += model.Answers1[i] + "&";
                    }
                    if (model.Answers1.Count > 0)
                        s = s.Remove(s.Length - 1);
                    qAnswer.StudentAnswer = s;
                }
                else if (model.Type == "Mapping")
                {
                    var answers = question.Answers2.Split('&').ToList();
                    if (answers.Count == model.Answers2.Count)
                    {
                        bool flag = true;
                        for (int i = 0; i < answers.Count; i++)
                        {
                            if (answers[i] != model.Answers2[i])
                                flag = false;
                        }

                        if (flag)
                            score++;

                        string s = "";
                        for (int i = 0; i < model.Answers2.Count; i++)
                        {
                            s += model.Answers2[i] + "&";
                        }
                        if (model.Answers2.Count > 0)
                            s = s.Remove(s.Length - 1);
                        qAnswer.StudentAnswer = s;
                    }
                }
                else
                    return NotFound();

                var existingAnswer = await _context.QuestionAnswer
                    .Include(x => x.Question)
                    .Include(x => x.Student)
                    .SingleOrDefaultAsync(x => x.Question.Id == question.Id && x.Student.Id == _userManager.GetUserId(User));

                if (existingAnswer == null)
                    _context.QuestionAnswer.Add(qAnswer);
                else
                {
                    existingAnswer.Type = qAnswer.Type;
                    existingAnswer.Task = qAnswer.Task;
                    existingAnswer.Answers1 = qAnswer.Answers1;
                    existingAnswer.Answers2 = qAnswer.Answers2;
                    existingAnswer.StudentAnswer = qAnswer.StudentAnswer;

                    _context.QuestionAnswer.Update(existingAnswer);
                }

                await _context.SaveChangesAsync();

                if (_context.Question.Any(x => x.SerialNum == model.SerialNum + 1))
                    return RedirectToAction("AnswerQuestion", new { score, moduleId = model.TestModuleId, serialNum = model.SerialNum + 1 });
                else
                {
                    TestScore testScore = new TestScore
                    {
                        Score = score,
                        ApplicationStudent = await _userManager.GetUserAsync(User),
                        TestDate = DateTime.Now,
                        TestModule = question.TestModule
                    };

                    _context.TestScore.Add(testScore);

                    await _context.SaveChangesAsync();

                    return RedirectToAction("TestResults", new { id = testScore.Id });
                }
            }
            return RedirectToAction("Details", "TestModules", new { id = model.TestModuleId });
        }

        [Authorize(Roles = "student")]
        public async Task<IActionResult> TestResults(int? id)
        {
            if (id == null)
                return NotFound();

            var testScore = await _context.TestScore
                .Include(x => x.TestModule)
                .Include(x => x.TestModule.TheoryModule)
                .Include(x => x.ApplicationStudent)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (testScore == null)
                return NotFound();

            TestResultViewModel model = new TestResultViewModel
            {
                Date = testScore.TestDate,
                Score = testScore.Score,
                TestModule = testScore.TestModule
            };

            model.Answers = new List<QuestionViewModel>();

            var answers = await _context.QuestionAnswer
                    .Include(x => x.Student)
                    .Include(x => x.Question)
                    .Include(x => x.Question.TestModule)
                    .Where(x => x.Student.Id == testScore.ApplicationStudent.Id && x.Question.TestModule.Id == testScore.TestModule.Id)
                    .ToListAsync();

            if (answers != null)
            {
                foreach (var a in answers)
                {
                    QuestionViewModel q = new QuestionViewModel
                    {
                        Task = a.Task,
                        Type = a.Type
                    };
                    if (a.Type == "Input")
                        q.Answers1 = new List<string>() { a.StudentAnswer };
                    else if (a.Type == "Choose")
                    {
                        q.Answers1 = (a.Answers1 + "&" + a.Answers2).Split('&').ToList();
                        q.Answers2 = a.StudentAnswer.Split('&').ToList();
                    }
                    else if (a.Type == "Mapping")
                    {
                        q.Answers1 = a.Answers1.Split('&').ToList();
                        q.Answers2 = a.StudentAnswer.Split('&').ToList();
                    }

                    model.Answers.Add(q);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "student")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(x => x.TestModule)
                .Include(x => x.TestModule.TheoryModule)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            QuestionViewModel model = new QuestionViewModel
            {
                Task = question.Task,
                TestModuleId = question.TestModule.Id,
                SerialNum = question.SerialNum,
                Type = question.Type,
                Answers1 = question.Answers1.Split('&').ToList(),
                Answers2 = String.IsNullOrWhiteSpace(question.Answers2) ? null : question.Answers2.Split('&').ToList()
            };

            ViewData["TheoryName"] = question.TestModule.TheoryModule.Name;

            return View(model);
        }

        [Authorize(Roles = "teacher")]
        public IActionResult Create(int? moduleId, int? serialNum, string type)
        {
            if (moduleId == null)
                return RedirectToAction("Index", "TestModules");

            if (!_context.TestModule.Any(x => x.Id == moduleId))
                return RedirectToAction("Index", "TestModules");

            if (serialNum == null)
            {
                if (_context.Question
                    .Include(x => x.TestModule)
                    .Any(x => x.TestModule.Id == moduleId))
                {
                    serialNum = _context.Question
                        .Include(x => x.TestModule)
                        .Where(x => x.TestModule.Id == moduleId)
                        .Max(x => x.SerialNum) + 1;
                }
                else
                    serialNum = 1;
            }

            QuestionViewModel model = new QuestionViewModel
            {
                SerialNum = (int)serialNum,
                TestModuleId = (int)moduleId
            };

            if (!string.IsNullOrWhiteSpace(type))
            {
                model.Type = type;
            }

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Create(QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var testModule = await _context.TestModule
                    .SingleOrDefaultAsync(x => x.Id == model.TestModuleId);
                if (testModule == null)
                    return NotFound();

                Question question = new Question
                {
                    Task = model.Task,
                    SerialNum = model.SerialNum,
                    TestModule = testModule
                };

                if (model.Type == "Input")
                {
                    if (model.Answers1.Count == 0)
                    {
                        ViewData["Error"] = "Введите правильный ответ";
                        return View(model);
                    }
                    question.Type = model.Type;
                    question.Answers1 = model.Answers1[0];
                }
                else if (model.Type == "Choose")
                {
                    if (model.Answers1.Count < 4)
                    {
                        ViewData["Error"] = "Введите 4 ответа";
                        return View(model);
                    }

                    if (model.Answers2 == null)
                    {
                        ViewData["Error"] = "Выберите один или более правильных ответов";
                        return View(model);
                    }

                    string s1 = "";
                    string s2 = "";
                    for (int i = 0; i < model.Answers1.Count; i++)
                    {
                        if (model.Answers2.Any(x => x == i.ToString()))
                        {
                            s1 += model.Answers1[i] + "&";
                        }
                        else
                        {
                            s2 += model.Answers1[i] + "&";
                        }
                    }
                    s1 = s1.Remove(s1.Length - 1);
                    question.Answers1 = s1;
                    s2 = s2.Remove(s2.Length - 1);
                    question.Answers2 = s2;

                    question.Type = model.Type;
                }
                else if (model.Type == "Mapping")
                {
                    model.Answers1 = model.Answers1.Where(x => !String.IsNullOrWhiteSpace(x)).ToList();
                    model.Answers2 = model.Answers2.Where(x => !String.IsNullOrWhiteSpace(x)).ToList();

                    if (model.Answers1.Count != model.Answers2.Count)
                    {
                        ViewData["Error"] = "Разное число вариантов для соответствия";
                        return View(model);
                    }
                    question.Type = model.Type;
                    string s1 = "";
                    string s2 = "";
                    for (int i = 0; i < model.Answers1.Count; i++)
                    {
                        s1 += model.Answers1[i] + "&";
                        s2 += model.Answers2[i] + "&";
                    }
                    s1 = s1.Remove(s1.Length - 1);
                    s2 = s2.Remove(s2.Length - 1);
                    question.Answers1 = s1;
                    question.Answers2 = s2;
                }
                else
                {
                    ViewData["Error"] = "Некорректный тип вопроса";
                    return View(model);
                }

                _context.Add(question);
                await _context.SaveChangesAsync();

                if (model.SerialNum == 9)
                    return RedirectToAction("Details", "TestModules", new { id = model.TestModuleId });

                return RedirectToAction("Create", new { moduleId = model.TestModuleId, serialNum = model.SerialNum + 1 });
            }
            return View(model);
        }

        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Edit(int? id, string type)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(x => x.TestModule)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            QuestionViewModel model = new QuestionViewModel
            {
                Task = question.Task,
                TestModuleId = question.TestModule.Id,
                SerialNum = question.SerialNum,
                Type = question.Type
            };

            if (model.Type == "Input")
            {
                string s = question.Answers1;
                ViewData["Answer"] = s;
                s = ViewData["Answer"].ToString();
            }
            else if (model.Type == "Choose" || model.Type == "Mapping")
            {
                string[] a1 = question.Answers1.Split('&');
                string[] a2 = question.Answers2.Split('&');

                

                model.Answers1 = question.Answers1.Split('&').ToList();
                model.Answers2 = String.IsNullOrWhiteSpace(question.Answers2) ? null : question.Answers2.Split('&').ToList();
            }

            ViewData["Type"] = model.Type;

            if (!String.IsNullOrWhiteSpace(type))
            {
                model.Type = type;
            }

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Edit(int id, QuestionViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var question = await _context.Question
                        .Include(x => x.TestModule)
                        .SingleOrDefaultAsync(x => x.Id == id);

                    if (model.Type == "Input")
                    {
                        if (model.Answers1.Count == 0)
                        {
                            ViewData["Error"] = "Введите правильный ответ";
                            return View(model);
                        }
                        question.Type = model.Type;
                        question.Answers1 = model.Answers1[0];
                        question.Answers2 = "";
                    }
                    else if (model.Type == "Choose")
                    {
                        if (model.Answers1.Count < 4)
                        {
                            ViewData["Error"] = "Введите ответы";
                            return View(model);
                        }

                        if (model.Answers2 == null)
                        {
                            ViewData["Error"] = "Выберите один или более правильных ответов";
                            return View(model);
                        }

                        string s1 = "";
                        string s2 = "";
                        for (int i = 0; i < model.Answers1.Count; i++)
                        {
                            if (model.Answers2.Any(x => x == i.ToString()))
                            {
                                s1 += model.Answers1[i] + "&";
                            }
                            else
                            {
                                s2 += model.Answers1[i] + "&";
                            }
                        }
                        s1 = s1.Remove(s1.Length - 1);
                        question.Answers1 = s1;
                        s2 = s2.Remove(s2.Length - 1);
                        question.Answers1 = s2;

                        question.Type = model.Type;
                    }
                    else if (model.Type == "Mapping")
                    {
                        model.Answers1 = model.Answers1.Where(x => !String.IsNullOrWhiteSpace(x)).ToList();
                        model.Answers2 = model.Answers2.Where(x => !String.IsNullOrWhiteSpace(x)).ToList();

                        if (model.Answers1.Count != model.Answers2.Count)
                        {
                            ViewData["Error"] = "Разное число вариантов для соответствия";
                            return View(model);
                        }
                        question.Type = model.Type;
                        string s1 = "";
                        string s2 = "";
                        for (int i = 0; i < model.Answers1.Count; i++)
                        {
                            s1 += model.Answers1[i] + "&";
                            s2 += model.Answers2[i] + "&";
                        }
                        s1 = s1.Remove(s1.Length - 1);
                        s2 = s2.Remove(s2.Length - 1);
                        question.Answers1 = s1;
                        question.Answers2 = s2;
                    }
                    else
                    {
                        ViewData["Error"] = "Некорректный тип вопроса";
                        return View(model);
                    }

                    question.Task = model.Task;

                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index",  new { id = model.TestModuleId });
            }
            return View(model);
        }

        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(x => x.TestModule)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            QuestionViewModel model = new QuestionViewModel
            {
                Task = question.Task,
                TestModuleId = question.TestModule.Id,
                SerialNum = question.SerialNum,
                Type = question.Type,
                Answers1 = question.Answers1.Split('&').ToList(),
                Answers2 = String.IsNullOrWhiteSpace(question.Answers2) ? null : question.Answers2.Split('&').ToList()
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Question
                .Include(x => x.TestModule)
                .SingleOrDefaultAsync(m => m.Id == id);

            _context.Question.Remove(question);

            var nextQuestions = _context.Question
                .Include(x => x.TestModule)
                .Where(x => x.TestModule.Id == question.TestModule.Id && x.SerialNum > question.SerialNum)
                .OrderBy(x => x.SerialNum);

            int counter = question.SerialNum;
            foreach (var q in nextQuestions)
            {
                q.SerialNum = counter;
                counter++;
                _context.Question.Update(q);
            }

            var answers = _context.QuestionAnswer
                .Include(x => x.Question)
                .Where(x => x.Question.Id == question.Id);

            foreach (var answer in answers)
            {
                _context.QuestionAnswer.Remove(answer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "TestModules", new { id = question.TestModule.Id });
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.Id == id);
        }
    }
}
