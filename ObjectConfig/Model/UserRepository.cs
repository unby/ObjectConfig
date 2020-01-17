using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using System.Threading.Tasks;

namespace ObjectConfig.Model
{
    public class UserRepository
    {
        public UserRepository(ObjectConfigContext configContext)
        {
            ConfigContext = configContext;
        }

        readonly ObjectConfigContext ConfigContext;

        public Task<User> GetUser(int internalId)
        {
            return ConfigContext.Users.Include(i => i.Environments).Include(i => i.Environments).FirstOrDefaultAsync(x => x.UserId == internalId);
        }

        public Task<User> GetUserByExternalId(string externalId)
        {
            return ConfigContext.Users.Include(i => i.Environments).Include(i => i.Environments).FirstOrDefaultAsync(x => x.ExternalId == externalId);
        }

        public async Task<User> CreateUser(User user)
        {
            var trackUser = ConfigContext.Users.Add(user);
            await ConfigContext.SaveChangesAsync();
            return trackUser.Entity;
        }
    }
}
