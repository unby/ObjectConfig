using System.Collections.Generic;

namespace ObjectConfig.Data
{
    public class User
    {
        public int UserId { get; set; }

        public string ExternalId { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public bool IsGlobalAdmin { get; set; }

        public List<UsersApplications> Applications { get; set; } = new List<UsersApplications>();

        public List<UsersEnvironments> Environments { get; set; } = new List<UsersEnvironments>();
    }
}
