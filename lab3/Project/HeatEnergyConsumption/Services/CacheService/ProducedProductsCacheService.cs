using Microsoft.Extensions.Caching.Memory;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Services.CacheService
{
    public class ProducedProductsCacheService : 
        DataCacheServiceFromDB<HeatEnergyConsumptionContext>, 
        ICacheService<ProducedProduct, string>
    {
        public ProducedProductsCacheService(HeatEnergyConsumptionContext dbContext,
            IMemoryCache cache, int rowsNumber = 20, int storageTime = 2 * 24 + 240) : 
            base(dbContext, cache, rowsNumber, storageTime) { }

        public void Add(string cacheKey)
        {
            IEnumerable<ProducedProduct> producedProducts = 
                dbContext.ProducedProducts.Take(rowsNumber).ToList();

            if (producedProducts != null)
                cache.Set(cacheKey, producedProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(storageTime)
                });
        }

        public IEnumerable<ProducedProduct> Get(string cacheKey)
        {
            IEnumerable<ProducedProduct> producedProducts;

            if (!cache.TryGetValue(cacheKey, out producedProducts))
            {
                Add(cacheKey);

                return cache.Get<IEnumerable<ProducedProduct>>(cacheKey);
            }

            return producedProducts;
        }
    }
}