namespace ObjectConfig.Data
{
    public class UsersTypes
    {
        public int UserId { get; protected set; }

        public User User { get; protected set; }

        public int ValueTypeId { get; protected set; }

        public TypeElement ValueType { get; protected set; }

        public TypeRole AccessRole { get; protected set; }
    }
}
