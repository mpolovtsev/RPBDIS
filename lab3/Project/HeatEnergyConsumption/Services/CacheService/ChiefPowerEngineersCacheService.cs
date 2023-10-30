using Microsoft.Extensions.Caching.Memory;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Services.CacheService
{
    public class ChiefPowerEngineersCacheService : 
        DataCacheServiceFromDB<HeatEnergyConsumptionContext>, 
        ICacheService<ChiefPowerEngineer, string>
    {
        public ChiefPowerEngineersCacheService(HeatEnergyConsumptionContext dbContext,
            IMemoryCache cache, int rowsNumber = 20, int storageTime = 2 * 24 + 240) : 
            base(dbContext, cache, rowsNumber, storageTime) { }

        public void Add(string cacheKey)
        {
            IEnumerable<ChiefPowerEngineer> chiefPowerEngineers = 
                dbContext.ChiefPowerEngineers.Take(rowsNumber).ToList();

            if (chiefPowerEngineers != null)
                cache.Set(cacheKey, chiefPowerEngineers, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(storageTime)
                });
        }

        public IEnumerable<ChiefPowerEngineer> Get(string cacheKey)
        {
            IEnumerable<ChiefPowerEngineer> chiefPowerEngineers;

            if (!cache.TryGetValue(cacheKey, out chiefPowerEngineers))
            {
                Add(cacheKey);

                return cache.Get<IEnumerable<ChiefPowerEngineer>>(cacheKey);
            }

            return chiefPowerEngineers;
        }
    }
}