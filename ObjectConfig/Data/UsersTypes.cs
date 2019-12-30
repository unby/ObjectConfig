using System;
using System.ComponentModel.DataAnnotations;

namespace ObjectConfig.Data
{
    public class UsersTypes
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int ValueTypeId { get; set; }
        
        public TypeElement ValueType { get; set; }

        public Role AccessRole { get; set; }

        public enum Role { SaveViewer, Viewer, SaveEditor, Editor }
    }
}
