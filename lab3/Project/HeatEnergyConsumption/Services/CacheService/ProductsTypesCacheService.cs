using Microsoft.Extensions.Caching.Memory;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Services.CacheService
{
    public class ProductsTypesCacheService : 
        DataCacheServiceFromDB<HeatEnergyConsumptionContext>, 
        ICacheService<ProductsType, string>
    {
        public ProductsTypesCacheService(HeatEnergyConsumptionContext dbContext,
            IMemoryCache cache, int rowsNumber = 20, int storageTime = 2 * 24 + 240) : 
            base(dbContext, cache, rowsNumber, storageTime) { }

        public void Add(string cacheKey)
        {
            IEnumerable<ProductsType> productsTypes = 
                dbContext.ProductsTypes.Take(rowsNumber).ToList();

            if (productsTypes != null)
                cache.Set(cacheKey, productsTypes, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(storageTime)
                });
        }

        public IEnumerable<ProductsType> Get(string cacheKey)
        {
            IEnumerable<ProductsType> productsTypes;

            if (!cache.TryGetValue(cacheKey, out productsTypes))
            {
                Add(cacheKey);

                return cache.Get<IEnumerable<ProductsType>>(cacheKey);
            }

            return productsTypes;
        }
    }
}