using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Model
{
    public class ConfigElementRepository
    {
        public ConfigElementRepository(ObjectConfigContext configContext)
        {
            _configContext = configContext;
        }

        private readonly ObjectConfigContext _configContext;

        public async Task<ConfigElement> Create(ConfigElement configElement)
        {
            configElement.Config.ConfigElement.Add(configElement);
            _configContext.Configs.Update(configElement.Config);
            await _configContext.SaveChangesAsync();
            return configElement;
        }

        public async Task<(ConfigElement root, ConfigElement[] all)> GetConfigElement(int id, CancellationToken token)
        {
            var all = await _configContext.ConfigElements.Include(i => i.Childs).Include(i => i.Type).Include(i => i.Value).Where(s => s.ConfigId == id).ToArrayAsync(token);
            var root = all.First(f => f.ParrentConfigElementId == null);
            return (root, all);
        }
    }
}
