using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using System.Threading.Tasks;

namespace ObjectConfig.Model
{
    public class UserRepository
    {
        public UserRepository(ObjectConfigContext configContext)
        {
            _configContext = configContext;
        }

        private readonly ObjectConfigContext _configContext;

        public Task<User> GetUser(int internalId)
        {
            return _configContext.Users.Include(i => i.Environments).Include(i => i.Environments).FirstOrDefaultAsync(x => x.UserId == internalId);
        }

        public Task<User> GetUserByExternalId(string externalId)
        {
            return _configContext.Users.Include(i => i.Environments).Include(i => i.Environments).FirstOrDefaultAsync(x => x.ExternalId == externalId);
        }

        public async Task<User> CreateUser(User user)
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User> trackUser = _configContext.Users.Add(user);
            await _configContext.SaveChangesAsync();
            return trackUser.Entity;
        }
    }
}
