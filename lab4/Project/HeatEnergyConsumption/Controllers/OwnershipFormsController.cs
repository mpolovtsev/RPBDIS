using Microsoft.AspNetCore.Mvc;
using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.Data;

namespace HeatEnergyConsumption.Controllers
{
    public class OwnershipFormsController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;

        public OwnershipFormsController(HeatEnergyConsumptionContext dbContext) =>
            this.dbContext = dbContext;

		[ResponseCache(CacheProfileName = "Caching")]
		public IActionResult Table()
        {
            IEnumerable<OwnershipForm> records = dbContext.OwnershipForms.OrderBy(record => record.Id);
            return View(records);
        }
    }
}