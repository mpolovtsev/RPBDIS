namespace HeatEnergyConsumption.Services.CacheService
{
    public interface ICacheService<T, K>
    {
        public void Add(T entity, K cacheKey);

        public void Update(T entity, K key);

        public T Get(int id);

        public IEnumerable<T> GetAll(K cacheKey);
    }
}