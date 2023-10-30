using Microsoft.Extensions.Caching.Memory;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Services.CacheService
{
    public class HeatEnergyConsumptionRatesCacheService : 
        DataCacheServiceFromDB<HeatEnergyConsumptionContext>, 
        ICacheService<HeatEnergyConsumptionRate, string>
    {
        public HeatEnergyConsumptionRatesCacheService(HeatEnergyConsumptionContext dbContext,
            IMemoryCache cache, int rowsNumber = 20, int storageTime = 2 * 24 + 240) : 
            base(dbContext, cache, rowsNumber, storageTime) { }

        public void Add(string cacheKey)
        {
            IEnumerable<HeatEnergyConsumptionRate> heatEnergyConsumptionRates = 
                dbContext.HeatEnergyConsumptionRates.Take(rowsNumber).ToList();

            if (heatEnergyConsumptionRates != null)
                cache.Set(cacheKey, heatEnergyConsumptionRates, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(storageTime)
                });
        }

        public IEnumerable<HeatEnergyConsumptionRate> Get(string cacheKey)
        {
            IEnumerable<HeatEnergyConsumptionRate> heatEnergyConsumptionRates;

            if (!cache.TryGetValue(cacheKey, out heatEnergyConsumptionRates))
            {
                Add(cacheKey);

                return cache.Get<IEnumerable<HeatEnergyConsumptionRate>>(cacheKey);
            }

            return heatEnergyConsumptionRates;
        }
    }
}