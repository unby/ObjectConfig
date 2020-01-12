using System;

namespace ObjectConfig.Data
{
    public class UsersApplications
    {
        public UsersApplications(int userId, int applicationId, Role accessRole)
        {
            UserId = userId;
            ApplicationId = applicationId;
            AccessRole = accessRole;
        }

        public UsersApplications(User? user, Application? application, Role accessRole) : this(user.UserId, application.ApplicationId, accessRole)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            Application = application ?? throw new ArgumentNullException(nameof(application));
        }

        public int UserId { get; protected set; }
       
        public virtual User User { get; protected set; }
        
        public int ApplicationId { get; protected set; }
        
        public virtual Application Application { get; protected set; }

        public Role AccessRole { get; protected set; }

        public enum Role { Viewer, Editor, Administrator }
    }
}
