using System;
using System.ComponentModel.DataAnnotations;

namespace ObjectConfig.Data
{
    public class UsersTypes
    {
        [Key]
        public Guid UserId { get; set; }
        [Key]
        public Guid ValueTypeId { get; set; }
        public virtual User User { get; set; }
        public virtual TypeElement ValueType { get; set; }

        public Role AccessRole { get; set; }
        public enum Role { SaveViewer, Viewer, SaveEditor, Editor }
    }
}
