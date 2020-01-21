using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using System.Linq;
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
            //  ConfigContext.Entry(configElement.Config)
            _configContext.Configs.Update(configElement.Config);
            await _configContext.SaveChangesAsync();
            return configElement;
        }

        public Task<ConfigElement> GetConfigElement(int id)
        {

            var t = _configContext.ConfigElements.Include(i => i.Childs).Include(i => i.Type).Include(i => i.Value).Where(s => s.ConfigId == id);
            return t.FirstOrDefaultAsync(s => s.ParrentConfigElementId == null);
        }
    }
}
