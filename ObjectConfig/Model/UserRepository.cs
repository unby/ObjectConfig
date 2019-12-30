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

        public User GetUser(int internalId)
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

    public class ConfigRepository
    {
        public ConfigRepository(ObjectConfigContext configContext)
        {
            ConfigContext = configContext;
        }

        readonly ObjectConfigContext ConfigContext;

        public Config CreateConfig(Config config)
        {
            var trackConfig = ConfigContext.Configs.Add(config);
            ConfigContext.SaveChanges();
            return trackConfig.Entity;
        }

        public Task<Config> Find(int id)
        {
            return ConfigContext.Configs.AsNoTracking().Include(i=>i.Environment).Include(i => i.ConfigElement).FirstOrDefaultAsync(f=>f.ConfigId==id);
        }

        public Task<Config> Find(string code)
        {
            return ConfigContext.Configs.AsNoTracking().Include(i => i.Environment).Include(i => i.ConfigElement).FirstOrDefaultAsync(f => f.Code == code);
        }
    }
}
