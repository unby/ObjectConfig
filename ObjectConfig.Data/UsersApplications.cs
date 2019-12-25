using System.ComponentModel.DataAnnotations;

namespace ObjectConfig.Data
{
    public class UsersApplications
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int ApplicationId { get; set; }
        public virtual User User { get; set; }
        public virtual Application Application { get; set; }

        public Role AccessRole { get; set; }
        public enum Role { Viewer, Editor, Administrtor }
    }
}
