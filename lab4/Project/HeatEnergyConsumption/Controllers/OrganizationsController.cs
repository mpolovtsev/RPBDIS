using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Controllers
{
	public class OrganizationsController : Controller
	{
		readonly HeatEnergyConsumptionContext dbContext;

		public OrganizationsController(HeatEnergyConsumptionContext dbContext) =>
			this.dbContext = dbContext;

		[ResponseCache(CacheProfileName = "Caching")]
		public IActionResult Table()
		{
			IEnumerable<Organization> records = dbContext.Organizations
				.Include(record => record.OwnershipForm)
				.Include(record => record.Manager)
				.OrderBy(record => record.Id);
			return View(records);
		}
	}
}