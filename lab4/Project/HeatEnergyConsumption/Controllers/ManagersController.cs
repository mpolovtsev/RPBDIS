using Microsoft.AspNetCore.Mvc;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Controllers
{
	public class ManagersController : Controller
	{
		readonly HeatEnergyConsumptionContext dbContext;

		public ManagersController(HeatEnergyConsumptionContext dbContext) =>
			this.dbContext = dbContext;

		[ResponseCache(CacheProfileName = "Caching")]
		public IActionResult Table()
		{
			IEnumerable<Manager> records = dbContext.Managers.OrderBy(record => record.Id);
			return View(records);
		}
	}
}