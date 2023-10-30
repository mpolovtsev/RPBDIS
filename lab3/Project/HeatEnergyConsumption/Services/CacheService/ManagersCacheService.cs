using Microsoft.Extensions.Caching.Memory;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Services.CacheService
{
    public class ManagersCacheService : 
        DataCacheServiceFromDB<HeatEnergyConsumptionContext>, 
        ICacheService<Manager, string>
    {
        public ManagersCacheService(HeatEnergyConsumptionContext dbContext,
            IMemoryCache cache, int rowsNumber = 20, int storageTime = 2 * 24 + 240) : 
            base(dbContext, cache, rowsNumber, storageTime) { }

        public void Add(string cacheKey)
        {
            IEnumerable<Manager> managers = 
                dbContext.Managers.Take(rowsNumber).ToList();

            if (managers != null)
                cache.Set(cacheKey, managers, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(storageTime)
                });
        }

        public IEnumerable<Manager> Get(string cacheKey)
        {
            IEnumerable<Manager> managers;

            if (!cache.TryGetValue(cacheKey, out managers))
            {
                Add(cacheKey);

                return cache.Get<IEnumerable<Manager>>(cacheKey);
            }

            return managers;
        }
    }
}