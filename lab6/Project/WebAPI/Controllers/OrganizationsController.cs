using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        readonly HeatEnergyConsumptionContext dbContext;

        public OrganizationsController(HeatEnergyConsumptionContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Получение списка всех организаций
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        public List<OrganizationViewModel> Get()
        {
            IEnumerable<Organization> organizations = dbContext.Organizations
                .Include(o => o.OwnershipForm)
                .Include(o => o.Manager);

            IEnumerable<OrganizationViewModel> organizationsViewModels = organizations.Select(o =>
                new OrganizationViewModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    OwnershipFormId = o.OwnershipFormId,
                    OwnershipFormName = o.OwnershipForm.Name,
                    Address = o.Address,
                    ManagerId = o.ManagerId,
                    ManagerSurname = o.Manager != null ? o.Manager.Surname : null
                });

            return organizationsViewModels.ToList();
        }

        /// <summary>
        /// Получение организации по id
        /// </summary>
        [HttpGet("{id}")]
        [Produces("application/json")]
        public IActionResult Get(int id)
        {
            Organization? organization = dbContext.Organizations
                .Include(o => o.OwnershipForm)
                .Include(o => o.Manager)
                .FirstOrDefault(o => o.Id == id);

            if (organization == null)
                return NotFound();

            OrganizationViewModel organizationViewModel = new OrganizationViewModel()
            {
                Id = organization.Id,
                Name = organization.Name,
                OwnershipFormId = organization.OwnershipFormId,
                OwnershipFormName = organization.OwnershipForm.Name,
                Address = organization.Address,
                ManagerId = organization.ManagerId,
                ManagerSurname = organization.Manager != null ? organization.Manager.Surname : null
            };

            return new ObjectResult(organizationViewModel);
        }

        /// <summary>
        /// Получение списка всех форм собственности
        /// </summary>
        [HttpGet("OwnershipForms")]
        [Produces("application/json")]
        public List<OwnershipForm> GetOwnershipForms()
        {
            return dbContext.OwnershipForms.ToList();
        }

        /// <summary>
        /// Получение списка всех руководителей
        /// </summary>
        [HttpGet("Managers")]
        [Produces("application/json")]
        public List<Manager> GetManagers()
        {
            return dbContext.Managers.ToList();
        }

        /// <summary>
        /// Добавление новой организации
        /// </summary>
        [HttpPost]
        [Produces("application/json")]
        public IActionResult Post([FromBody] Organization organization)
        {
            if (organization == null)
                return BadRequest();

            dbContext.Organizations.Add(organization);
            dbContext.SaveChanges();

            return Ok(organization);
        }

        /// <summary>
        /// Обновление существующей организации
        /// </summary>
        [HttpPut]
        [Produces("application/json")]
        public IActionResult Put([FromBody] Organization organization)
        {
            if (organization == null)
                return BadRequest();

            if (!dbContext.Organizations.Any(o => o.Id == organization.Id))
                return NotFound();

            dbContext.Update(organization);
            dbContext.SaveChanges();

            return Ok(organization);
        }

        /// <summary>
        /// Удаление существующей организации
        /// </summary>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        public IActionResult Delete(int id)
        {
            Organization? organization = dbContext.Organizations.FirstOrDefault(o => o.Id == id);

            if (organization == null)
                return NotFound();

            dbContext.Organizations.Remove(organization);
            dbContext.SaveChanges();

            return Ok(organization);
        }
    }
}