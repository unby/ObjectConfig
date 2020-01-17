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
            ConfigContext = configContext;
            ConfigElementRepository = configElementRepository;
        }

        readonly ObjectConfigContext ConfigContext;
        readonly ConfigElementRepository ConfigElementRepository;

        public Config CreateConfig(Config config)
        {
            var trackConfig = ConfigContext.Configs.Add(config);
            ConfigContext.SaveChanges();
            return trackConfig.Entity;
        }

        public Task<Config> Find(int id)
        {
            var result = ConfigContext.Configs.AsNoTracking().FirstOrDefaultAsync(f => f.ConfigId == id);

            return result;
        }

        public Task<Config> Find(string code)
        {
            return ConfigContext.Configs.AsNoTracking().FirstOrDefaultAsync(f => f.Code == code && f.DateFrom > DateTimeOffset.UtcNow && (f.DateTo == null) && f.VersionTo == null);
        }
    }
}
