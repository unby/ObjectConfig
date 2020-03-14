namespace ObjectConfig.Data
{
    public class UsersEnvironments : IRole<EnvironmentRole>
    {
        private UsersEnvironments()
        {
        }

        public UsersEnvironments(User user, Environment environment, EnvironmentRole accessRole)
            : this(user.UserId, environment.EnvironmentId, accessRole)
        {
            User = user;
            Environment = environment;
        }

        public UsersEnvironments(int userId, int environmentId, EnvironmentRole accessRole)
        {
            UserId = userId;
            EnvironmentId = environmentId;
            AccessRole = accessRole;
        }

        public int UserId { get; protected set; }

        public virtual User User { get; protected set; }

        public int EnvironmentId { get; protected set; }

        public virtual Environment Environment { get; protected set; }

        public EnvironmentRole AccessRole { get; set; }
    }
}
