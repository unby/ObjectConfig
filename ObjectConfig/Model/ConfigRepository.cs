using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using System;
using System.Threading.Tasks;

namespace ObjectConfig.Model
{
    public class ConfigRepository
    {
        public ConfigRepository(ObjectConfigContext configContext, ConfigElementRepository configElementRepository)
        {
            _configContext = configContext;
            _configElementRepository = configElementRepository;
        }

        private readonly ObjectConfigContext _configContext;
        private readonly ConfigElementRepository _configElementRepository;

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
