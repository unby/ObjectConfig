using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public User GetUser(Guid internalId)
        {
            return ConfigContext.Users.Include(i=>i.Environments).Include(i => i.Environments).FirstOrDefault(x => x.UserId == internalId);
        }

        public User CreateUser(User user)
        {
            var trackUser= ConfigContext.Users.Add(user);
            ConfigContext.SaveChanges();
            return trackUser.Entity;
        }
    }

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
    }
}
