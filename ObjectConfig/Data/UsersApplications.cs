using System;

namespace ObjectConfig.Data
{
    public class UsersApplications
    {
        public int UserId { get; set; }
       
        public virtual User User { get; set; }
        
        public int ApplicationId { get; set; }
        
        public virtual Application Application { get; set; }

        public Role AccessRole { get; set; }

        public enum Role { Viewer, Editor, Administrator }
    }
}
