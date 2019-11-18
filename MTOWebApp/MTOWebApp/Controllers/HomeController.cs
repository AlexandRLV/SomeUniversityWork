using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MTOWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MTOWebApp.Data;
using Microsoft.EntityFrameworkCore;
using MTOWebApp.Models.ManageViewModels;
using MTOWebApp.Models.ModulesViewModels;

namespace MTOWebApp.Controllers
{
    // Основной контроллер - навигация, профиль пользователя
    public class HomeController : Controller
    {
        // Ссылка для работы с бд
        private readonly ApplicationDbContext _context;

        // Менеджер пользователей
        private readonly UserManager<ApplicationUser> _userManager;

        // Конструктор, используем иньекцию зависимостей
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Главная страница
        public IActionResult Index()
        {
            return View();
        }

        // Страница с описанием системы
        public IActionResult About()
        {
            ViewData["Message"] = "Описание системы.";

            return View();
        }

        // Страница с контактной информацией
        public IActionResult Contact()
        {
            ViewData["Message"] = "Контактная информация.";

            return View();
        }

        // Список пользователей с ролью "преподаватель". Только для администратора.
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> TeacherList()
        {
            // Создаём пустой список
            var teachers = new List<ApplicationUser>();

            // Пробегаемся по всем пользователям
            foreach (var u in _userManager.Users)
            {
                // Если пользователь в роли "преподаватель", но не "админ", то добавляем его в список
                if (await _userManager.IsInRoleAsync(u, "teacher") && !(await _userManager.IsInRoleAsync(u, "admin")))
                {
                    teachers.Add(u);
                }
            }

            return View(teachers);
        }

        // Список студентов. Только для препода (и админа)
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> StudentList()
        {
            // Создаём пустой список
            var students = new List<ApplicationUser>();

            // Пробегаемся по всем пользователям
            foreach (var u in _userManager.Users)
            {
                // Если пользователь в роли "студент", но не  в роли "преподаватель", добавляем его
                if (await _userManager.IsInRoleAsync(u, "student") && !(await _userManager.IsInRoleAsync(u, "teacher")))
                {
                    students.Add(u);
                }
            }

            return View(students);
        }

        // Страница профиля. Только для авторизованных
        [Authorize]
        public async Task<IActionResult> Profile(string email)
        {
            // Проверяем пришедший email, если он пустой, возвращаем ошибку
            if (String.IsNullOrWhiteSpace(email))
                return NotFound();

            // Пробуем найти пользователя с таким email, если его нет, возвращаем ошибку
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Email == email);
            if (user == null)
                return NotFound();

            // Запоминаем, в какой роли находится пользователь
            if (await _userManager.IsInRoleAsync(user, "admin"))
            {
                ViewData["role"] = "admin";
            }
            else if (await _userManager.IsInRoleAsync(user, "teacher"))
            {
                ViewData["role"] = "teacher";
            }
            else if (await _userManager.IsInRoleAsync(user, "student"))
            {
                ViewData["role"] = "student";
            }
            else
            {
                ViewData["role"] = "none";
            }

            // Создаём новую модель представления, записываем туда данные пользователя
            IndexViewModel model = new IndexViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                MiddleName = user.MiddleName,
                Email = user.Email
            };

            // Ищем информацию о последнем тестировании пользователя
            var lastTest = await _context.TestScore
                .Include(x => x.ApplicationStudent)
                .Include(x => x.TestModule)
                .Where(x => x.ApplicationStudent.Email == email)
                .OrderByDescending(x => x.TestDate)
                .FirstOrDefaultAsync();

            // И записываем её в модель
            model.LastTest = lastTest;

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
