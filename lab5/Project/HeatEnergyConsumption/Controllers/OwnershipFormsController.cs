using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.Controllers
{
    [Authorize]
    public class OwnershipFormsController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        const int pageSize = 10;

        public OwnershipFormsController(HeatEnergyConsumptionContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index(string name,
            SortState sortOrder = SortState.OwnershipFormNameAsc, int page = 1)
        {
            IQueryable<OwnershipForm> ownershipForms = dbContext.OwnershipForms;

            if (ownershipForms == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (HttpContext.Request.Method == "GET" && HttpContext.Request.Cookies.TryGetValue("OwneshipFormName", out string? nameCookie))
            {
                ownershipForms = Filter(ownershipForms, nameCookie);
                ViewData["OwneshipFormName"] = nameCookie;
            }
            else if (HttpContext.Request.Method == "POST" && !string.IsNullOrEmpty(name))
            {
                ownershipForms = Filter(ownershipForms, name);
                HttpContext.Response.Cookies.Append("OwneshipFormName", name);
            }

            // Сортировка
            ownershipForms = Sort(ownershipForms, sortOrder);

            // Разбиение на страницы
            int count = await ownershipForms.CountAsync();
            ownershipForms = ownershipForms.Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            // Формирование модели для передачи представлению
            OwnershipFormsViewModel model = new OwnershipFormsViewModel()
            {
                OwnershipForms = ownershipForms,
                Name = name,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.OwnershipForms == null)
                return NotFound();

            var ownershipForm = await dbContext.OwnershipForms
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ownershipForm == null)
                return NotFound();

            return View(ownershipForm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] OwnershipForm ownershipForm)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(ownershipForm);
                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(ownershipForm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.OwnershipForms == null)
                return NotFound();

            var ownershipForm = await dbContext.OwnershipForms.FindAsync(id);

            if (ownershipForm == null)
                return NotFound();

            return View(ownershipForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] OwnershipForm ownershipForm)
        {
            if (id != ownershipForm.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(ownershipForm);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnershipFormExists(ownershipForm.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(ownershipForm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.OwnershipForms == null)
                return NotFound();

            var ownershipForm = await dbContext.OwnershipForms
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ownershipForm == null)
                return NotFound();

            return View(ownershipForm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.OwnershipForms == null)
                return NotFound();

            var ownershipForm = await dbContext.OwnershipForms.FindAsync(id);

            if (ownershipForm == null)
                return NotFound();

            dbContext.OwnershipForms.Remove(ownershipForm);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool OwnershipFormExists(int id)
        {
            return (dbContext.OwnershipForms?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        IQueryable<OwnershipForm> Filter(IQueryable<OwnershipForm> ownershipForms, string? name)
        {
            return ownershipForms.Where(ownershipForm => ownershipForm.Name.Contains(name ?? ""));
        }

        IQueryable<OwnershipForm> Sort(IQueryable<OwnershipForm> ownershipForms, SortState sortOrder = SortState.OwnershipFormNameAsc)
        {
            ViewData["NameSort"] = sortOrder == SortState.OwnershipFormNameAsc ? SortState.OwnershipFormNameDesc : SortState.OwnershipFormNameAsc;

            return sortOrder switch
            {
                SortState.OwnershipFormNameDesc => ownershipForms.OrderByDescending(ownershipForm => ownershipForm.Name),
                _ => ownershipForms.OrderBy(ownershipForm => ownershipForm.Name)
            };
        }
    }
}