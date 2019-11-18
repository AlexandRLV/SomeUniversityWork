using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    public class ParagraphsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ParagraphsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Paragraph
                .OrderBy(x => x.TheoryModule.Id)
                .ThenBy(x => x.SerialNum)
                .Include(x => x.TheoryModule)
                .ToListAsync());
        }

        [Authorize(Roles = "student")]
        public async Task<IActionResult> IndexToModule(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var hiddenModules = new List<int>();
            
            if (!String.IsNullOrEmpty(user.HiddenThemes))
            {
                hiddenModules = user.HiddenThemes.Split('&').Select(x => int.Parse(x)).ToList();
            }

            if (hiddenModules.Contains(id))
                return NotFound();

            var module = _context.TheoryModule.SingleOrDefault(x => x.Id == id);
            if (module == null)
            {
                return NotFound();
            }

            ViewData["ModuleName"] = module.Name;
            ViewData["ModuleId"] = module.Id;

            return View(await _context.Paragraph.Where(x => x.TheoryModule.Id == id).Include(x => x.TheoryModule).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p = await _context.Paragraph.Include(x => x.TheoryModule)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (p == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var hiddenModules = new List<int>();

            if (!String.IsNullOrEmpty(user.HiddenThemes))
            {
                hiddenModules = user.HiddenThemes.Split('&').Select(x => int.Parse(x)).ToList();
            }

            if (hiddenModules.Contains(p.TheoryModule.Id))
                return NotFound();

            using (StreamReader sr = new StreamReader($"wwwroot/Paragraphs/{p.TheoryModule.Id}/{p.Id}/text.txt", Encoding.GetEncoding(1251)))
            {
                ViewData["Data"] = sr.ReadToEnd();
            }

            ViewData["ModuleId"] = p.TheoryModule.Id;
            ViewData["ModuleName"] = p.TheoryModule.Name;

            if (!String.IsNullOrWhiteSpace(p.PictureFileName))
            {
                ViewData["PicturePath"] = $"Paragraphs/{p.TheoryModule.Id}/{p.Id}/{p.PictureFileName}";
            }
            if (!String.IsNullOrWhiteSpace(p.SoundFileName))
            {
                ViewData["AudioPath"] = $"Paragraphs/{p.TheoryModule.Id}/{p.Id}/{p.SoundFileName}";
            }
            if (!String.IsNullOrWhiteSpace(p.VideoFileName))
            {
                ViewData["VideoPath"] = $"Paragraphs/{p.TheoryModule.Id}/{p.Id}/{p.VideoFileName}";
            }

            ViewData["Next"] = "none";
            ViewData["Prev"] = "none";
            if (p.SerialNum < 3)
            {
                var next = _context.Paragraph.SingleOrDefault(x => x.TheoryModule.Id == p.TheoryModule.Id && x.SerialNum == p.SerialNum + 1);
                if (next != null)
                    ViewData["Next"] = next.Id;
            }
            if (p.SerialNum > 1)
            {
                var prev = _context.Paragraph.SingleOrDefault(x => x.TheoryModule.Id == p.TheoryModule.Id && x.SerialNum == p.SerialNum - 1);
                if (prev != null)
                    ViewData["Prev"] = prev.Id;
            }

            return View(p);
        }

        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Create(int? moduleId, int? serialNum)
        {
            if (moduleId == null)
            {
                return RedirectToAction("Create", "TheoryModules");
            }

            var module = await _context.TheoryModule.SingleOrDefaultAsync(m => m.Id == moduleId);
            if (module == null)
                return RedirectToAction("Create", "TheoryModules");

            if (serialNum == null)
            {
                var paragraphs = _context.Paragraph.Where(x => x.TheoryModule.Id == moduleId).OrderBy(x => x.SerialNum);
                if (!paragraphs.Any(x => x.SerialNum == 1))
                    serialNum = 1;
                else if (!paragraphs.Any(x => x.SerialNum == 2))
                    serialNum = 2;
                else if (!paragraphs.Any(x => x.SerialNum == 3))
                    serialNum = 3;
                else
                    return RedirectToAction("Details", "TheoryModules", new { id = moduleId });
            }

            ParagraphViewModel model = new ParagraphViewModel
            {
                ModuleId = (int)moduleId,
                SerialNum = (int)serialNum
            };

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Create(ParagraphViewModel model)
        {
            if (ModelState.IsValid)
            {
                var module = _context.TheoryModule.SingleOrDefault(x => x.Id == model.ModuleId);
                if (module == null)
                    return RedirectToAction("Create", "TheoryModules");

                if (_context.Paragraph.Any(x => x.Name == model.Name && x.TheoryModule.Id == model.ModuleId))
                {
                    ViewData["Error"] = "Имя уже занято.";
                    return View(model);
                }

                Paragraph p = new Paragraph
                {
                    Name = model.Name,
                    TheoryModule = _context.TheoryModule.SingleOrDefault(x => x.Id == model.ModuleId),
                    SerialNum = model.SerialNum
                };

                if (model.Picture != null)
                    p.PictureFileName = model.Picture.FileName;

                if (model.Audio != null)
                    p.SoundFileName = model.Audio.FileName;

                if (model.Video != null)
                    p.VideoFileName = model.Video.FileName;


                _context.Add(p);
                await _context.SaveChangesAsync();

                if (!Directory.Exists($"wwwroot/Paragraphs/{module.Id}"))
                {
                    Directory.CreateDirectory($"wwwroot/Paragraphs/{module.Id}");
                }
                if (!Directory.Exists($"wwwroot/Paragraphs/{module.Id}/{p.Id}"))
                {
                    Directory.CreateDirectory($"wwwroot/Paragraphs/{module.Id}/{p.Id}");
                }

                using (StreamWriter sw = new StreamWriter($"wwwroot/Paragraphs/{module.Id}/{p.Id}/text.txt", false, Encoding.GetEncoding(1251)))
                {
                    sw.Write(model.Text);
                }

                if (model.Picture != null)
                {
                    using (FileStream fs = new FileStream($"wwwroot/Paragraphs/{module.Id}/{p.Id}/{model.Picture.FileName}", FileMode.Create))
                    {
                        await model.Picture.CopyToAsync(fs);
                    }
                }

                if (model.Audio != null)
                {
                    using (FileStream fs = new FileStream($"wwwroot/Paragraphs/{module.Id}/{p.Id}/{model.Audio.FileName}", FileMode.Create))
                    {
                        await model.Audio.CopyToAsync(fs);
                    }
                }

                if (model.Video != null)
                {
                    using (FileStream fs = new FileStream($"wwwroot/Paragraphs/{module.Id}/{p.Id}/{model.Video.FileName}", FileMode.Create))
                    {
                        await model.Video.CopyToAsync(fs);
                    }
                }

                if (model.SerialNum < 3)
                    return RedirectToAction("Create", new { moduleId = model.ModuleId, serialNum = model.SerialNum + 1 });
                else
                    return RedirectToAction("Details", "TheoryModules", new { id = model.ModuleId });
            }
            return View(model);
        }

        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paragraph = await _context.Paragraph.Include(x => x.TheoryModule).SingleOrDefaultAsync(m => m.Id == id);
            if (paragraph == null)
            {
                return NotFound();
            }

            ParagraphViewModel model = new ParagraphViewModel
            {
                Name = paragraph.Name,
                ModuleId = paragraph.TheoryModule.Id,
                SerialNum = paragraph.SerialNum
            };

            using (StreamReader sr = new StreamReader($"wwwroot/Paragraphs/{paragraph.TheoryModule.Id}/{paragraph.Id}/text.txt", Encoding.GetEncoding(1251)))
            {
                ViewData["Data"] = sr.ReadToEnd();
            }

            if (!String.IsNullOrWhiteSpace(paragraph.PictureFileName))
            {
                ViewData["PicturePath"] = $"Paragraphs/{paragraph.TheoryModule.Id}/{paragraph.Id}/{paragraph.PictureFileName}";
            }
            else
            {
                ViewData["PicturePath"] = "none";
            }

            if (!String.IsNullOrWhiteSpace(paragraph.SoundFileName))
            {
                ViewData["AudioPath"] = $"Paragraphs/{paragraph.TheoryModule.Id}/{paragraph.Id}/{paragraph.SoundFileName}";
            }
            else
            {
                ViewData["AudioPath"] = "none";
            }

            if (!String.IsNullOrWhiteSpace(paragraph.VideoFileName))
            {
                ViewData["VideoPath"] = $"Paragraphs/{paragraph.TheoryModule.Id}/{paragraph.Id}/{paragraph.VideoFileName}";
            }
            else
            {
                ViewData["VideoPath"] = "none";
            }

            ViewData["Pid"] = paragraph.Id;

            return View(model);
        }

        [Authorize(Roles = "teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ParagraphViewModel model)
        {
            if (!ParagraphExists(id))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var p = _context.Paragraph.Include(x => x.TheoryModule).SingleOrDefault(x => x.Id == id);

                    p.Name = model.Name;

                    using (StreamWriter sw = new StreamWriter($"wwwroot/Paragraphs/{p.TheoryModule.Id}/{p.Id}/text.txt", false, Encoding.GetEncoding(1251)))
                    {
                        sw.Write(model.Text);
                    }

                    // Проверяем, изменена ли картинка
                    if (model.Picture != null)
                    {
                        // Если да, то удаляем старую
                        if (p.PictureFileName != null && p.PictureFileName != model.Picture.FileName)
                        {
                            System.IO.File.Delete($"wwwroot/Paragraphs/{p.TheoryModule.Id}/{p.Id}/{p.PictureFileName}");
                        }

                        // И сохраняем новую
                        using (FileStream fs = new FileStream($"wwwroot/Paragraphs/{p.TheoryModule.Id}/{p.Id}/{model.Picture.FileName}", FileMode.Create))
                        {
                            await model.Picture.CopyToAsync(fs);
                        }
                        p.PictureFileName = model.Picture.FileName;
                    }

                    // Проверяем, изменена ли аудио-дорожка
                    if (model.Audio != null)
                    {
                        // Если да, то удаляем старую
                        if (p.SoundFileName != null && p.SoundFileName != model.Audio.FileName)
                        {
                            System.IO.File.Delete($"wwwroot/Paragraphs/{p.TheoryModule.Id}/{p.Id}/{p.SoundFileName}");
                        }

                        // И сохраняем новую
                        using (FileStream fs = new FileStream($"wwwroot/Paragraphs/{p.TheoryModule.Id}/{p.Id}/{model.Audio.FileName}", FileMode.Create))
                        {
                            await model.Audio.CopyToAsync(fs);
                        }
                        p.SoundFileName = model.Audio.FileName;
                    }

                    // Проверяем, изменено ли видео
                    if (model.Video != null)
                    {
                        // Если да, то удаляем старое
                        if (p.VideoFileName != null && p.VideoFileName != model.Video.FileName)
                        {
                            System.IO.File.Delete($"wwwroot/Paragraphs/{p.TheoryModule.Id}/{p.Id}/{p.VideoFileName}");
                        }

                        // И сохраняем новое
                        using (FileStream fs = new FileStream($"wwwroot/Paragraphs/{p.TheoryModule.Id}/{p.Id}/{model.Video.FileName}", FileMode.Create))
                        {
                            await model.Video.CopyToAsync(fs);
                        }
                        p.VideoFileName = model.Video.FileName;
                    }

                    _context.Update(p);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
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

            var paragraph = await _context.Paragraph.Include(x => x.TheoryModule)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (paragraph == null)
            {
                return NotFound();
            }

            return View(paragraph);
        }

        [Authorize(Roles = "teacher")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paragraph = await _context.Paragraph.Include(x => x.TheoryModule).SingleOrDefaultAsync(m => m.Id == id);
            _context.Paragraph.Remove(paragraph);
            await _context.SaveChangesAsync();
            Directory.Delete($"wwwroot/Paragraphs/{paragraph.TheoryModule.Id}/{paragraph.Id}", true);

            return RedirectToAction("Details", "TheoryModules", new { id = paragraph.TheoryModule.Id });
        }

        private bool ParagraphExists(int id)
        {
            return _context.Paragraph.Any(e => e.Id == id);
        }
    }
}
