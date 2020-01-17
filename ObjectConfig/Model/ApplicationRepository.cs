using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectConfig.Model
{
    public class ApplicationRepository
    {
        private readonly ObjectConfigContext _configContext;

        public ApplicationRepository(ObjectConfigContext configContext)
        {
            _configContext = configContext;
        }

        public async Task Create(Application app)
        {
            _configContext.Applications.Add(app);
            await _configContext.SaveChangesAsync();
        }

        public Task<Application> Find(string code)
        {
            return _configContext.Applications.Include(k => k.Users).Include(k => k.Environments).Include(i => i.Users).FirstOrDefaultAsync(f => f.Code == code);
        }

        public Task<Application> Find(int id)
        {
            return _configContext.Applications.Include(k => k.Users).Include(k => k.Environments).Include(i => i.Users).FirstOrDefaultAsync(f => f.ApplicationId == id);
        }

        public Task<List<UsersApplications>> FindByUser(int id)
        {
            return _configContext.UsersApplications.Include(k => k.Application).Where(f => f.UserId == id).ToListAsync();
        }

        public Task<int> Update(Application app)
        {
            _configContext.Attach(app);
            _configContext.Entry(app);
            return _configContext.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var author = new Application(id);
            _configContext.Applications.Remove(author);
            return await _configContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(string code)
        {
            _configContext.Applications.Remove(await _configContext.Applications.FirstOrDefaultAsync(f => f.Code == code));
            return await _configContext.SaveChangesAsync() > 0;
        }
    }
}
