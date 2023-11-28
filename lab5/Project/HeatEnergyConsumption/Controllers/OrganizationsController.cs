using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.PageViewModels;
using HeatEnergyConsumption.ViewModels;
using System.Xml.Linq;

namespace HeatEnergyConsumption.Controllers
{
    [Authorize]
    public class OrganizationsController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        const int pageSize = 10;

        public OrganizationsController(HeatEnergyConsumptionContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index(string name, string address, string ownershipForm, string manager, SortState sortOrder = SortState.OrganizationNameAsc, int page = 1)
        {
            IQueryable<Organization> organizations = dbContext.Organizations
                .Include(organization => organization.Manager)
                .Include(organization => organization.OwnershipForm);

            if (organizations == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("OrganizationName", out string? nameCookie);
                HttpContext.Request.Cookies.TryGetValue("OrganizationAddress", out string? addressCookie);
                HttpContext.Request.Cookies.TryGetValue("OrganizationOwnershipForm", out string? ownershipFormCookie);
                HttpContext.Request.Cookies.TryGetValue("OrganizationManager", out string? managerCookie);

                if (!(string.IsNullOrEmpty(nameCookie) && string.IsNullOrEmpty(addressCookie) && string.IsNullOrEmpty(ownershipFormCookie) &&
                    string.IsNullOrEmpty(managerCookie)))
                {
                    organizations = Filter(organizations, nameCookie, addressCookie, ownershipFormCookie, managerCookie);
                    ViewData["OrganizationName"] = nameCookie;
                    ViewData["OrganizationAddress"] = addressCookie;
                    ViewData["OrganizationOwnershipForm"] = ownershipFormCookie;
                    ViewData["OrganizationManagerCookie"] = managerCookie;
                }
            }
            else if (HttpContext.Request.Method == "POST" && !(string.IsNullOrEmpty(name) && string.IsNullOrEmpty(address) &&
                string.IsNullOrEmpty(ownershipForm) && string.IsNullOrEmpty(manager)))
            {
                organizations = Filter(organizations, name, address, ownershipForm, manager);

                if (name != null)
                    HttpContext.Response.Cookies.Append("OrganizationName", name);

                if (address != null)
                    HttpContext.Response.Cookies.Append("OrganizationAddress", address);

                if (ownershipForm != null)
                    HttpContext.Response.Cookies.Append("OrganizationOwnershipForm", ownershipForm);

                if (manager != null)
                    HttpContext.Response.Cookies.Append("OrganizationManager", manager);
            }

            // Сортировка
            organizations = Sort(organizations, sortOrder);

            // Разбиение на страницы
            int count = await organizations.CountAsync();
            organizations = organizations.Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            OrganizationsViewModel model = new OrganizationsViewModel()
            {
                Organizations = organizations,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.Organizations == null)
                return NotFound();

            var organization = await dbContext.Organizations
                .Include(o => o.Manager)
                .Include(o => o.OwnershipForm)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (organization == null)
                return NotFound();

            return View(organization);
        }

        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(dbContext.Managers, "Id", "Surname");
            ViewData["OwnershipFormId"] = new SelectList(dbContext.OwnershipForms, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,OwnershipFormId,Address,ManagerId")] Organization organization)
        {
            if (ModelState.ErrorCount <= 2)
            {
                dbContext.Add(organization);
                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ManagerId"] = new SelectList(dbContext.Managers, "Id", "Surname", organization.ManagerId);
            ViewData["OwnershipFormId"] = new SelectList(dbContext.OwnershipForms, "Id", "Name", organization.OwnershipFormId);

            return View(organization);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.Organizations == null)
                return NotFound();

            var organization = await dbContext.Organizations.FindAsync(id);

            if (organization == null)
                return NotFound();

            ViewData["ManagerId"] = new SelectList(dbContext.Managers, "Id", "Id", organization.ManagerId);
            ViewData["OwnershipFormId"] = new SelectList(dbContext.OwnershipForms, "Id", "Id", organization.OwnershipFormId);

            return View(organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,OwnershipFormId,Address,ManagerId")] Organization organization)
        {
            if (id != organization.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(organization);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ManagerId"] = new SelectList(dbContext.Managers, "Id", "Id", organization.ManagerId);
            ViewData["OwnershipFormId"] = new SelectList(dbContext.OwnershipForms, "Id", "Id", organization.OwnershipFormId);

            return View(organization);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.Organizations == null)
                return NotFound();

            var organization = await dbContext.Organizations
                .Include(o => o.Manager)
                .Include(o => o.OwnershipForm)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (organization == null)
                return NotFound();

            return View(organization);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.Organizations == null)
                return NotFound();

            var organization = await dbContext.Organizations.FindAsync(id);

            if (organization == null)
                return NotFound();

            dbContext.Organizations.Remove(organization);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool OrganizationExists(int id)
        {
            return (dbContext.Organizations?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        IQueryable<Organization> Filter(IQueryable<Organization> organizations, string? name, string? address, string? ownershipForm, string? manager)
        {
            return organizations.Where(organization => organization.Name.Contains(name ?? "") && organization.Address.Contains(address ?? "") &&
            organization.OwnershipForm.Name.Contains(ownershipForm ?? "") && organization.Manager.Surname.Contains(manager ?? ""));
        }

        IQueryable<Organization> Sort(IQueryable<Organization> organizations, SortState sortOrder = SortState.OrganizationNameAsc)
        {
            ViewData["NameSort"] = sortOrder == SortState.OrganizationNameAsc ? SortState.OrganizationNameDesc : SortState.OrganizationNameAsc;
            ViewData["ManagerSort"] = sortOrder == SortState.OrganizationManagerAsc ? SortState.OrganizationManagerDesc : SortState.OrganizationManagerAsc;

            return sortOrder switch
            {
                SortState.OrganizationNameDesc => organizations.OrderByDescending(organization => organization.Name),
                SortState.OrganizationManagerAsc => organizations.OrderBy(organization => organization.Manager.Surname),
                SortState.OrganizationManagerDesc => organizations.OrderByDescending(organization => organization.Manager.Surname),
                _ => organizations.OrderBy(organization => organization.Name)
            };
        }
    }
}
