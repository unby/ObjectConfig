using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using System.Threading.Tasks;

namespace ObjectConfig.Model
{
    public class ApplicationRepository
    {
        public ApplicationRepository(ObjectConfigContext configContext)
        {
            ConfigContext = configContext;
        }

        readonly ObjectConfigContext ConfigContext;

        public async Task Create(Application app)
        {
            ConfigContext.Applications.Add(app);
            await ConfigContext.SaveChangesAsync();
        }

        public Task<Application> Find(string code)
        {
            return ConfigContext.Applications.Include(k => k.Environments).Include(i => i.Users).FirstOrDefaultAsync(f => f.Code == code);
        }

        public Task<Application> Find(int id)
        {
            return ConfigContext.Applications.Include(k => k.Environments).Include(i => i.Users).FirstOrDefaultAsync(f => f.ApplicationId == id);
        }
    }
}
