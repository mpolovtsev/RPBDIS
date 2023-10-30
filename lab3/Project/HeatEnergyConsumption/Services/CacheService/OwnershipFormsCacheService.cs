using Microsoft.Extensions.Caching.Memory;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Services.CacheService
{
    public class OwnershipFormsCacheService : 
        DataCacheServiceFromDB<HeatEnergyConsumptionContext>, 
        ICacheService<OwnershipForm, string>
    {
        public OwnershipFormsCacheService(HeatEnergyConsumptionContext dbContext, 
            IMemoryCache cache, int rowsNumber = 20, int storageTime = 2 * 24 + 240) :
            base(dbContext, cache, rowsNumber, storageTime) { }

        public void Add(string cacheKey)
        {
            IEnumerable<OwnershipForm> ownershipForms = 
                dbContext.OwnershipForms.Take(rowsNumber).ToList();

            if (ownershipForms != null)
                cache.Set(cacheKey, ownershipForms, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(storageTime)
                });
        }

        public IEnumerable<OwnershipForm> Get(string cacheKey)
        {
            IEnumerable<OwnershipForm> ownershipForms;

            if (!cache.TryGetValue(cacheKey, out ownershipForms))
            {
                Add(cacheKey);

                return cache.Get<IEnumerable<OwnershipForm>>(cacheKey);
            }

            return ownershipForms;
        }
    }
}