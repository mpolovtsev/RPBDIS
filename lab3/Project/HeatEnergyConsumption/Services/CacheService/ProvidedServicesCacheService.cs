using Microsoft.Extensions.Caching.Memory;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Services.CacheService
{
    public class ProvidedServicesCacheService : 
        DataCacheServiceFromDB<HeatEnergyConsumptionContext>, 
        ICacheService<ProvidedService, string>
    {
        public ProvidedServicesCacheService(HeatEnergyConsumptionContext dbContext,
            IMemoryCache cache, int rowsNumber = 20, int storageTime = 2 * 24 + 240) : 
            base(dbContext, cache, rowsNumber, storageTime) { }

        public void Add(string cacheKey)
        {
            IEnumerable<ProvidedService> providedServices = 
                dbContext.ProvidedServices.Take(rowsNumber).ToList();

            if (providedServices != null)
                cache.Set(cacheKey, providedServices, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(storageTime)
                });
        }

        public IEnumerable<ProvidedService> Get(string cacheKey)
        {
            IEnumerable<ProvidedService> providedServices;

            if (!cache.TryGetValue(cacheKey, out providedServices))
            {
                Add(cacheKey);

                return cache.Get<IEnumerable<ProvidedService>>(cacheKey);
            }

            return providedServices;
        }
    }
}