using System.Collections.Generic;

namespace ObjectConfig.Data
{
    public class Application
    {
        public int ApplicationId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public virtual IList<Environment> Environments { get; set; } = new List<Environment>();

        public virtual IList<UsersApplications> Users { get; set; } = new List<UsersApplications>();
    }
}
