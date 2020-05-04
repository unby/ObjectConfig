namespace ObjectConfig.Data
{
    public class UsersTypes
    {
        public int UserId { get; protected set; }

        public User User { get; protected set; }

        public int TypeElementId { get; protected set; }

        public TypeElement TypeElement { get; protected set; }

        public TypeRole AccessRole { get; protected set; }
    }
}
