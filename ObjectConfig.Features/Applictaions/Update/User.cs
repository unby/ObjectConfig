using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Users;

namespace ObjectConfig.Features.Applictaions.Update
{
    public class User
           : IUserAcessLevel<ApplicationRole>
    {
        public User(int userId, ApplicationRole role, EntityOperation operation)
        {
            if (userId < 1)
            {
                throw new RequestException($"Parameter '{nameof(userId)}' isn't correct  value");
            }

            UserId = userId;
            Role = role;
            Operation = operation;
        }

        public int UserId { get; }
        public ApplicationRole Role { get; }
        public EntityOperation Operation { get; }
    }
}
