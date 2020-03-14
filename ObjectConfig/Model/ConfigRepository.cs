using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using System;
using System.Threading.Tasks;

namespace ObjectConfig.Model
{
    public class ConfigRepository
    {
        private readonly ObjectConfigContext _configContext;

        public ConfigRepository(ObjectConfigContext configContext)
        {
            _configContext = configContext;
        }

        public Config CreateConfig(Config config)
        {
            var trackConfig = _configContext.Configs.Add(config);
            _configContext.SaveChanges();
            return trackConfig.Entity;
        }

        public Task<Config> Find(int id)
        {
            var result = _configContext.Configs.AsNoTracking().FirstOrDefaultAsync(f => f.ConfigId == id);

            return result;
        }

        public Task<Config> Find(string code)
        {
            return _configContext.Configs.AsNoTracking().FirstOrDefaultAsync(f => f.Code == code && f.DateFrom > DateTimeOffset.UtcNow && (f.DateTo == null) && f.VersionTo == null);
        }
    }
}
