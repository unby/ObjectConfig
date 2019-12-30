namespace ObjectConfig.Data
{
    public class UsersEnvironments
    {
        public int UserId { get; set; }
        
        public virtual User User { get; set; }

        public int EnvironmentId { get; set; }
        
        public virtual Environment Environment { get; set; }
        
        public Role AccessRole { get; set; }

        public enum Role { Viewer, TargetEditor, Editor }
    }
}
