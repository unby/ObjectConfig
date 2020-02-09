using ObjectConfig.Data;
using ObjectConfig.Exceptions;

namespace ObjectConfig.Features.Users
{
    public class AccessCardOfUser : IAccessCardOfUser
    {
        public AccessCardOfUser(int userId, User.Role userRole)
        {
            if (userId < 1)
            {
                throw new OperationException($"{nameof(userId)} must be greater than zero");
            }

            UserId = userId;
            UserRole = userRole;
        }

        public int UserId { get; }
        public User.Role UserRole { get; }
    }
}
