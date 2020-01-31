using ObjectConfig.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjectConfig.Model
{
    public class UsersApplicationsRepository
    {
        private readonly ObjectConfigContext _configContext;

        public UsersApplicationsRepository(ObjectConfigContext configContext)
        {
            _configContext = configContext;
        }

        public async Task<List<UsersApplications>> Insert(List<UsersApplications> usersApplications)
        {
            _configContext.UsersApplications.AddRange(usersApplications);
            await _configContext.SaveChangesAsync();
            return usersApplications;
        }

        public async Task<List<UsersApplications>> Update(List<UsersApplications> usersApplications)
        {
            _configContext.UsersApplications.UpdateRange(usersApplications);
            await _configContext.SaveChangesAsync();
            return usersApplications;
        }

        public async Task Delete(List<UsersApplications> usersApplications)
        {
            _configContext.UsersApplications.RemoveRange(usersApplications);
            await _configContext.SaveChangesAsync();
        }
    }
}
