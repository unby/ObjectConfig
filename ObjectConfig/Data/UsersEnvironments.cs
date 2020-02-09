namespace ObjectConfig.Data
{
    public class UsersEnvironments
    {
        private UsersEnvironments()
        {
        }

        public UsersEnvironments(User user, Environment environment, Role accessRole)
            : this(user.UserId, environment.EnvironmentId, accessRole)
        {
            User = user;
            Environment = environment;
        }

        public UsersEnvironments(int userId, int environmentId, Role accessRole)
        {
            UserId = userId;
            EnvironmentId = environmentId;
            AccessRole = accessRole;
        }

        public int UserId { get; protected set; }

        public virtual User User { get; protected set; }

        public int EnvironmentId { get; protected set; }

        public virtual Environment Environment { get; protected set; }

        public Role AccessRole { get; protected set; }

        public enum Role { Viewer, TargetEditor, Editor }
    }
}
