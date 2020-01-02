using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using System;
using System.Linq;
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
            var result = ConfigContext.Configs.AsNoTracking().FirstOrDefaultAsync(f=>f.ConfigId==id);

            return result;
        }

        public Task<Config> Find(string code)
        {
            return ConfigContext.Configs.AsNoTracking().FirstOrDefaultAsync(f => f.Code == code && f.DateFrom > DateTimeOffset.UtcNow && (f.DateTo == null) && f.VersionTo == null);
        }
    }

    public class ConfigElementRepository
    {
        public ConfigElementRepository(ObjectConfigContext configContext)
        {
            ConfigContext = configContext;
        }

        readonly ObjectConfigContext ConfigContext;

        public async Task<ConfigElement> Create(ConfigElement configElement)
        {
            configElement.Config.ConfigElement.Add(configElement);
          //  ConfigContext.Entry(configElement.Config)
            ConfigContext.Configs.Update(configElement.Config);
            await ConfigContext.SaveChangesAsync();
            return configElement;
        }

        public ConfigElement GetConfigElement(int id) {

            var t = ConfigContext.ConfigElements.Include(i => i.Childs).Include(i => i.Type).Include(i => i.Value).Where(s => s.ConfigId == id).ToList();
            return t.FirstOrDefault(s => s.ParrentConfigElementId == null);
        }
    }
}
