using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectConfig.Model
{
    public class UserRepository
    {
        public UserRepository(ObjectConfigContext configContext)
        {
            ConfigContext = configContext;
        }

        readonly ObjectConfigContext ConfigContext;

        public User GetUser(int internalId)
        {
            return ConfigContext.Users.Include(i=>i.Environments).Include(i => i.Environments).FirstOrDefault(x => x.UserId == internalId);
        }

        public User GetUserByExternalId(string externalId)
        {
            return ConfigContext.Users.Include(i => i.Environments).Include(i => i.Environments).FirstOrDefault(x => x.ExternalId == externalId);
        }

        public User CreateUser(User user)
        {
            var trackUser= ConfigContext.Users.Add(user);
            ConfigContext.SaveChanges();
            return trackUser.Entity;
        }
    }
}
