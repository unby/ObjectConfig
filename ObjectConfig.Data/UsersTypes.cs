using System.ComponentModel.DataAnnotations;

namespace ObjectConfig.Data
{
    public class UsersTypes
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int ValueTypeId { get; set; }
        public virtual User User { get; set; }
        public virtual ValueType ValueType { get; set; }

        public Role AccessRole { get; set; }
        public enum Role { SaveViewer, Viewer, SaveEditor, Editor }
    }
}
