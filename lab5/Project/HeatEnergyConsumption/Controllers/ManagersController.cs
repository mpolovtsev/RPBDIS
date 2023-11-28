using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;
using Microsoft.AspNetCore.Authorization;
using HeatEnergyConsumption.ViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.Controllers
{
    [Authorize]
    public class ManagersController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        const int pageSize = 10;

        public ManagersController(HeatEnergyConsumptionContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index(string name, string surname, string middleName,
            SortState sortOrder = SortState.ManagerNameAsc, int page = 1)
        {
            IQueryable<Manager> managers = dbContext.Managers;

            if (managers == null)
                return Problem("Записи не найдены.");

            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("ManagerName", out string? nameCookie);
                HttpContext.Request.Cookies.TryGetValue("ManagerSurame", out string? surnameCookie);
                HttpContext.Request.Cookies.TryGetValue("ManagerMiddleName", out string? middleNameCookie);

                if (!(string.IsNullOrEmpty(nameCookie) && string.IsNullOrEmpty(surnameCookie) && string.IsNullOrEmpty(middleNameCookie)))
                {
                    managers = Filter(managers, nameCookie, surnameCookie, middleNameCookie);
                    ViewData["ManagerName"] = nameCookie;
                    ViewData["ManagerSurname"] = surnameCookie;
                    ViewData["ManagerMiddlename"] = middleNameCookie;
                }
            }
            else if (HttpContext.Request.Method == "POST" && !(string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname) &&
                string.IsNullOrEmpty(middleName)))
            {
                managers = Filter(managers, name, surname, middleName);

                if (name != null)
                    HttpContext.Response.Cookies.Append("ManagerName", name);

                if (surname != null)
                    HttpContext.Response.Cookies.Append("ManagerSurame", surname);

                if (middleName != null)
                    HttpContext.Response.Cookies.Append("ManagerMiddleName", middleName);
            }

            // Сортировка
            managers = Sort(managers, sortOrder);

            // Разбиение на страницы
            int count = await managers.CountAsync();
            managers = managers.Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            ManagersViewModel model = new ManagersViewModel()
            {
                Managers = managers,
                Name = name,
                Surname = surname,
                MiddleName = middleName,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.Managers == null)
                return NotFound();

            var manager = await dbContext.Managers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manager == null)
                return NotFound();

            return View(manager);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,MiddleName,PhoneNumber")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(manager);
                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(manager);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.Managers == null)
                return NotFound();

            var manager = await dbContext.Managers.FindAsync(id);

            if (manager == null)
                return NotFound();

            return View(manager);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,MiddleName,PhoneNumber")] Manager manager)
        {
            if (id != manager.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(manager);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(manager);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.Managers == null)
                return NotFound();

            var manager = await dbContext.Managers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manager == null)
                return NotFound();

            return View(manager);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.Managers == null)
                return NotFound();

            var manager = await dbContext.Managers.FindAsync(id);

            if (manager == null)
                return NotFound();

            dbContext.Managers.Remove(manager);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool ManagerExists(int id)
        {
            return (dbContext.Managers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        IQueryable<Manager> Filter(IQueryable<Manager> managers, string? name, string? surname, string? middleName)
        {
            return managers.Where(manager => manager.Name.Contains(name ?? "") && manager.Surname.Contains(surname ?? "") && manager.MiddleName.Contains(middleName ?? ""));
        }

        IQueryable<Manager> Sort(IQueryable<Manager> managers, SortState sortOrder = SortState.ManagerNameAsc)
        {
            ViewData["NameSort"] = sortOrder == SortState.ManagerNameAsc ? SortState.ManagerNameDesc : SortState.ManagerNameAsc;
            ViewData["SurnameSort"] = sortOrder == SortState.ManagerSurnameAsc ? SortState.ManagerSurnameDesc : SortState.ManagerSurnameAsc;
            ViewData["MiddleNameSort"] = sortOrder == SortState.ManagerMiddleNameAsc ? SortState.ManagerMiddleNameDesc : SortState.ManagerMiddleNameAsc;

            return sortOrder switch
            {
                SortState.ManagerNameDesc => managers.OrderByDescending(manager => manager.Name),
                SortState.ManagerSurnameAsc => managers.OrderBy(manager => manager.Surname),
                SortState.ManagerSurnameDesc => managers.OrderByDescending(manager => manager.Surname),
                SortState.ManagerMiddleNameAsc => managers.OrderBy(manager => manager.MiddleName),
                SortState.ManagerMiddleNameDesc => managers.OrderByDescending(manager => manager.MiddleName),
                _ => managers.OrderBy(manager => manager.Name)
            };
        }
    }
}
