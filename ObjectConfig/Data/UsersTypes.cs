using System;
using System.ComponentModel.DataAnnotations;

namespace ObjectConfig.Data
{
    public class UsersTypes
    {
        public int UserId { get; protected set; }

        public User User { get; protected set; }

        public int ValueTypeId { get; protected set; }
        
        public TypeElement ValueType { get; protected set; }

        public Role AccessRole { get; protected set; }

        public enum Role { SaveViewer, Viewer, SaveEditor, Editor }
    }
}
