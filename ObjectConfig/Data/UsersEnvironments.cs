namespace ObjectConfig.Data
{
    public class UsersEnvironments
    {
        public int UserId { get; protected set; }

        public virtual User User { get; protected set; }

        public int EnvironmentId { get; protected set; }

        public virtual Environment Environment { get; protected set; }

        public Role AccessRole { get; protected set; }

        public enum Role { Viewer, TargetEditor, Editor }
    }
}
