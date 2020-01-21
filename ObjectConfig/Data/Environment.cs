using System.Collections.Generic;

namespace ObjectConfig.Data
{
    public class Environment
    {
        public int EnvironmentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public IList<Config> Configs { get; set; } = new List<Config>();
        public IList<UsersEnvironments> Users { get; set; } = new List<UsersEnvironments>();
    }
}
