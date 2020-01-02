using System;

namespace ObjectConfig.Data
{
    public class UsersApplications
    {
        public int UserId { get; protected set; }
       
        public virtual User User { get; protected set; }
        
        public int ApplicationId { get; protected set; }
        
        public virtual Application Application { get; protected set; }

        public Role AccessRole { get; protected set; }

        public enum Role { Viewer, Editor, Administrator }
    }
}
