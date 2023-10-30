using Microsoft.Extensions.Caching.Memory;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Services.CacheService
{
    public class ServicesTypesCacheService : 
        DataCacheServiceFromDB<HeatEnergyConsumptionContext>, 
        ICacheService<ServicesType, string>
    {
        public ServicesTypesCacheService(HeatEnergyConsumptionContext dbContext,
            IMemoryCache cache, int rowsNumber = 20, int storageTime = 2 * 24 + 240) : 
            base(dbContext, cache, rowsNumber, storageTime) { }

        public void Add(string cacheKey)
        {
            IEnumerable<ServicesType> servicesTypes = 
                dbContext.ServicesTypes.Take(rowsNumber).ToList();

            if (servicesTypes != null)
                cache.Set(cacheKey, servicesTypes, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(storageTime)
                });
        }

        public IEnumerable<ServicesType> Get(string cacheKey)
        {
            IEnumerable<ServicesType> servicesTypes;

            if (!cache.TryGetValue(cacheKey, out servicesTypes))
            {
                Add(cacheKey);

                return cache.Get<IEnumerable<ServicesType>>(cacheKey);
            }

            return servicesTypes;
        }
    }
}