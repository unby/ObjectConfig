using System.ComponentModel.DataAnnotations;

namespace ObjectConfig.Data
{
    public class UsersEnvironments
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int EnvironmentId { get; set; }
        public virtual User User { get; set; }
        public virtual Environment Environment { get; set; }

        public Role AccessRole { get; set; }

        public enum Role { Viewer, TargetEditor, Editor }
    }
}
