using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTOWebApp.Data;
using MTOWebApp.Models;
using MTOWebApp.Models.ModulesViewModels;

namespace MTOWebApp.Controllers
{
    // Контроллер модулей с теорией
    public class TheoryModulesController : Controller
    {
        // Ссылка для работы с бд
        private readonly ApplicationDbContext _context;

        // Менеджер пользователей
        private readonly UserManager<ApplicationUser> _userManager;

        public TheoryModulesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Страница со списком всех модулей
        public async Task<IActionResult> Index()
        {
            // Получаем текущего пользователя
            var user = await _userManager.GetUserAsync(User);
            
            // Создаём список скрытых от него тем
            var hiddenModules = new List<int>();

            // Записываем в список скрытые темы, которые хранятся в бд
            if (!String.IsNullOrEmpty(user.HiddenThemes))
            {
                hiddenModules = user.HiddenThemes.Split('&').Select(x => int.Parse(x)).ToList();
            }

            // Получаем список модулей, которые не скрыты от текущего пользователя
            var modules = await _context.TheoryModule
                .Where(x => !hiddenModules.Contains(x.Id))
                .Select(x => new TheoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Paragraphs = _context.Paragraph.Count(y => y.TheoryModule.Id == x.Id)
                })
                .ToListAsync();

            return View(modules);
        }

        // Страница просмотра информации о модуле
        public async Task<IActionResult> Details(int? id)
        {
            // Проверяем пришедшую ссылку
            if (id == null)
            {
                return NotFound();
            }

            // Получаем текущего пользователя и список скрытых для него тем
            var user = await _userManager.GetUserAsync(User);
            var hiddenModules = new List<int>();

            // Если модуль скрыт от пользователя, выдаём ошибку
            if (!String.IsNullOrEmpty(user.HiddenThemes))
            {
                hiddenModules = user.HiddenThemes.Split('&').Select(x => int.Parse(x)).ToList();

                if (hiddenModules.Contains((int)id))
                {
                    return NotFound();
                }
            }

            // Получаем нужный модуль
            var theoryModule = await _context.TheoryModule
                .SingleOrDefaultAsync(m => m.Id == id);
            if (theoryModule == null)
            {
                return NotFound();
            }

            // Создаём модель представления и записываем информацию
            TheoryViewModel model = new TheoryViewModel
            {
                Id = (int)id,
                Name = theoryModule.Name,
                Description = theoryModule.Description,
                Paragraphs = _context.Paragraph.Count(x => x.TheoryModule.Id == theoryModule.Id)
            };

            // Ищем модуль с тестированием по этой теме
            var testModule = await _context.TestModule
                .Include(x => x.TheoryModule)
                .SingleOrDefaultAsync(x => x.TheoryModule.Id == id);

            // Если такой нашёлся, записываем его
            if (testModule == null)
                model.TestId = null;
            else
                model.TestId = testModule.Id;

            return View(model);
        }

        // Страница создания модуля с теорией. Только для учителей
        [Authorize(Roles = "teacher")]
        public IActionResult Create()
        {
            TheoryViewModel model = new TheoryViewModel();
            return View(model);
        }

        // Метод создания модуля по введённой информации. Только для учителей
        [Authorize(Roles = "teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TheoryViewModel model)
        {
            int id = -1;
            if (ModelState.IsValid)
            {
                // Создаём новый модуль
                TheoryModule module = new TheoryModule()
                {
                    Name = model.Name,
                    Description = model.Description
                };
                
                // Сохраняем его в бд
                _context.Add(module);
                await _context.SaveChangesAsync();

                // Перенаправляем пользователя на создание параграфов
                id = module.Id;
                return RedirectToAction("Create", "Paragraphs", new { moduleId = id, serialNum = 1 });
            }
            return RedirectToAction(nameof(Index));
        }

        // Страница редактирования модуля
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            // Проверяем ссылку
            if (id == null)
            {
                return NotFound();
            }

            // Пробуем найти нужный модуль
            var theoryModule = await _context.TheoryModule.SingleOrDefaultAsync(m => m.Id == id);
            if (theoryModule == null)
            {
                return NotFound();
            }

            // Создаём модель представления и записываем информацию
            TheoryViewModel model = new TheoryViewModel
            {
                Id = theoryModule.Id,
                Name = theoryModule.Name,
                Description = theoryModule.Description,
                Paragraphs = _context.Paragraph.Count(x => x.TheoryModule.Id == theoryModule.Id)
            };

            return View(model);
        }

        [Authorize(Roles = "teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TheoryViewModel model)
        {
            if (!TheoryModuleExists(id) || id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var module = _context.TheoryModule.SingleOrDefault(x => x.Id == id);

                    module.Name = model.Name;
                    module.Description = model.Description;

                    _context.Update(module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Details", new { id });
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

            var theoryModule = await _context.TheoryModule
                .SingleOrDefaultAsync(m => m.Id == id);
            if (theoryModule == null)
            {
                return NotFound();
            }

            // Создаём модель представления и записываем информацию
            TheoryViewModel model = new TheoryViewModel
            {
                Id = theoryModule.Id,
                Name = theoryModule.Name,
                Description = theoryModule.Description,
                Paragraphs = _context.Paragraph.Count(x => x.TheoryModule.Id == theoryModule.Id)
            };

            return View(model);
        }

        [Authorize(Roles = "teacher")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var theoryModule = await _context.TheoryModule
                .SingleOrDefaultAsync(m => m.Id == id);

            var paragraphs = _context.Paragraph
                .Include(x => x.TheoryModule)
                .Where(x => x.TheoryModule.Id == id);

            var testModule = _context.TestModule
                .Include(x => x.TheoryModule)
                .SingleOrDefault(x => x.TheoryModule.Id == id);

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

            _context.TheoryModule.Remove(theoryModule);
            _context.TestModule.Remove(testModule);
            foreach (var p in paragraphs)
                _context.Remove(p);
            foreach (var q in questions)
                _context.Question.Remove(q);
            foreach (var a in answers)
                _context.Remove(a);
            foreach (var s in scores)
                _context.Remove(s);

            await _context.SaveChangesAsync();
            Directory.Delete($"wwwroot/Paragraphs/{theoryModule.Id}", true);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> HideTheme(int? id)
        {
            // Если ИД пустой, ошибка
            if (id == null)
                return NotFound();

            // Получаем нужный модуль по его ИД
            var module = _context.TheoryModule.SingleOrDefault(x => x.Id == id);
            if (module == null)
                return NotFound();

            // Передаём в представление название темы
            ViewData["ModuleName"] = module.Name;

            // Получаем список всех студентов и тех, для которых эта тема скрыта
            var students = new List<ApplicationUser>();
            foreach (var user in _userManager.Users)
            {
                if (!(await _userManager.IsInRoleAsync(user, "teacher")))
                {
                    students.Add(user);
                }
            }
            var hiddenUsers = students.Where(x => x.HiddenThemes.Split('&').Any(t => t == id.ToString())).Select(x => x.Id).ToList();

            // Создаём модель, в которую записываем ИД темы и списки, полученные ранее
            HideThemeViewModel model = new HideThemeViewModel
            {
                ModuleId = (int)id,
                AllUsers = students,
                HiddenUsers = hiddenUsers
            };

            // Передаём модель в представление
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> HideTheme(HideThemeViewModel model)
        {
            // Получаем список всех студентов и тех, для которых эта тема скрыта
            var students = new List<ApplicationUser>();
            foreach (var user in _userManager.Users)
            {
                if (!(await _userManager.IsInRoleAsync(user, "teacher")))
                {
                    students.Add(user);
                }
            }
            var hiddenUsers = students.Where(x => x.HiddenThemes.Split('&').Any(t => t == model.ModuleId.ToString())).Select(x => x.Id).ToList();

            if (model.HiddenUsers == null)
                model.HiddenUsers = new List<string>();

            // Получаем пользователей, для которых эта тема стала скрытой
            var addedUsers = model.HiddenUsers.Except(hiddenUsers);

            // Получаем пользователей, для которых эта тема перестала быть скрытой
            var removedUsers = hiddenUsers.Except(model.HiddenUsers);

            foreach (string id in addedUsers)
            {
                // Получаем пользователя по ИД
                var user = students.SingleOrDefault(x => x.Id == id);

                // Если пользователь не найден, продолжаем
                if (user == null)
                    continue;

                if (String.IsNullOrEmpty(user.HiddenThemes))
                {
                    // Если список скрытых для него тем - пустой, добавляем туда ИД этой темы
                    user.HiddenThemes = model.ModuleId.ToString();
                    _context.Update(user);
                }
                else if (!user.HiddenThemes.Split('&').Any(t => t == model.ModuleId.ToString()))
                {
                    // Если в списке скрытых для него тем не было этой темы раньше, добавляем её в конец.
                    user.HiddenThemes += "&" + model.ModuleId;
                    _context.Update(user);
                }
            }

            foreach (string id in removedUsers)
            {
                // Получаем пользователя по ИД
                var user = students.SingleOrDefault(x => x.Id == id);

                // Если пользователь не найден, продолжаем
                if (user == null)
                    continue;

                // Если нет скрытых от него тем, пропускаем его
                if (String.IsNullOrEmpty(user.HiddenThemes))
                    continue;

                // Разбиваем строку из БД на список скрытых тем
                var hiddenThemes = user.HiddenThemes.Split('&').ToList();
                if (hiddenThemes.Any(t => t == model.ModuleId.ToString()))
                {
                    // Если эта тема для него скрыта, удаляем её из списка
                    hiddenThemes.Remove(model.ModuleId.ToString());

                    if (hiddenThemes.Count == 0)
                    {
                        // Если не осталось больше скрытых тем, сохраняем в БД пустую строку
                        user.HiddenThemes = "";
                    }
                    else
                    {
                        // Если есть другие скрытые темы, составляем новую строку, которая будет храниться в БД
                        string s = "";
                        for (int i = 0; i < hiddenThemes.Count - 1; i++)
                        {
                            s += hiddenThemes[i] + "&";
                        }
                        s += hiddenThemes[hiddenThemes.Count - 1];
                        user.HiddenThemes = s;
                    }

                    _context.Update(user);
                }
            }

            await _context.SaveChangesAsync();

            // Возвращаемся к просмотру темы
            return RedirectToAction("Details", new { id = model.ModuleId });
        }

        private bool TheoryModuleExists(int id)
        {
            return _context.TheoryModule.Any(e => e.Id == id);
        }
    }
}
