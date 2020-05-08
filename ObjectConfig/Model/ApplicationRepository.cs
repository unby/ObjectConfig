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

        public Task<Application> Find(string code)
        {
            return _configContext.Applications.Include(k => k.Users).Include(k => k.Environments).Include(i => i.Users).FirstOrDefaultAsync(f => f.Code == code);
        }

        public Task<List<UsersApplications>> FindByUser(int id)
        {
            return _configContext.UsersApplications.Include(k => k.Application).Where(f => f.UserId == id).ToListAsync();
        }
    }
}
