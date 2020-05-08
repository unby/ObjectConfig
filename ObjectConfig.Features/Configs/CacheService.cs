using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;

namespace ObjectConfig.Features.Configs
{
    public class CacheService
    {
        private readonly ObjectConfigContext _objectConfigContext;
        private const string JsonType = "json";
        public CacheService(ObjectConfigContext objectConfigContext)
        {
            _objectConfigContext = objectConfigContext;
        }

        private IQueryable<ConfigCache> CacheQuery(int configId, string type)
        {
            return _objectConfigContext.ConfigCache.Where(w => w.ConfigId == configId && w.OutType == type);
        }

        public async Task<string> GetGonfig(int configId,
            CancellationToken token = default(CancellationToken),
            string type = JsonType)
        {
            return (await CacheQuery(configId, type).SingleAsync(token)).ConfigValue;
        }

        public async Task UpdateJsonConfig(int configId, string data,
            CancellationToken token = default(CancellationToken), string type = JsonType)
        {
            var cache = await CacheQuery(configId, type).SingleAsync(token);
            cache.UpdateValue(data);
            await _objectConfigContext.SaveChangesAsync(token);
        }

        public async Task AddJsonConfig(int configId, string data,
            CancellationToken token = default(CancellationToken), string type = JsonType)
        {
            _objectConfigContext.ConfigCache.Add(new ConfigCache(configId, data, type));
            await _objectConfigContext.SaveChangesAsync(token);
        }
    }
}
