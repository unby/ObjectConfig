using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Common;

namespace ObjectConfig.Features.Users
{
    public class AccessCardOfUser : IAccessCardOfUser
    {
        public AccessCardOfUser(int userId, UserRole userRole)
        {
            if (userId < 1)
            {
                throw new OperationException($"{nameof(userId)} must be greater than zero");
            }

            UserId = userId;
            UserRole = userRole;
        }

        public int UserId { get; }
        public UserRole UserRole { get; }
    }
}
