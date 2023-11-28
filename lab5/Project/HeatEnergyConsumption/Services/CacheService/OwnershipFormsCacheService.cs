using Microsoft.Extensions.Caching.Memory;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HeatEnergyConsumption.Services.CacheService
{
    public class OwnershipFormsCacheService : 
        DataCacheServiceFromDB<HeatEnergyConsumptionContext>, 
        ICacheService<OwnershipForm, string>
    {
        public OwnershipFormsCacheService(HeatEnergyConsumptionContext dbContext, IMemoryCache cache,
            int storageTime = 600) : base(dbContext, cache, storageTime) { }

        public async void Add(OwnershipForm ownershipForm, string key)
        {
            await dbContext.OwnershipForms.AddAsync(ownershipForm);
            int n = await dbContext.SaveChangesAsync();

            if (n > 0)
                cache.Set(key, dbContext.OwnershipForms, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(storageTime)
                });
        }

        public void Update(OwnershipForm entity, string key)
        {
            throw new NotImplementedException();
        }

        public OwnershipForm Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OwnershipForm> GetAll(string cacheKey)
        {
            IEnumerable<OwnershipForm> ownershipForms;

            if (!cache.TryGetValue(cacheKey, out ownershipForms))
            {
                UpdateCache(cacheKey);

                return cache.Get<IEnumerable<OwnershipForm>>(cacheKey);
            }

            return ownershipForms;
        }

        void UpdateCache(string cacheKey)
        {
            cache.Set(cacheKey, dbContext.OwnershipForms, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(storageTime)
            });
        }
    }
}