using System.Collections.Generic;

namespace ObjectConfig.Data
{
    public class User
    {
        private User()
        {
        }

        public User(int userId, string externalId, string displayName, string email, bool isGlobalAdmin)
        {
            UserId = userId;
            ExternalId = externalId;
            DisplayName = displayName;
            Email = email;
            IsGlobalAdmin = isGlobalAdmin;
        }

        public int UserId { get; protected set; }

        public string ExternalId { get; protected set; }

        public string DisplayName { get; protected set; }

        public string Email { get; protected set; }

        public bool IsGlobalAdmin { get; protected set; }

        public List<UsersApplications> Applications { get; set; } = new List<UsersApplications>();

        public List<UsersEnvironments> Environments { get; set; } = new List<UsersEnvironments>();
    }
}
