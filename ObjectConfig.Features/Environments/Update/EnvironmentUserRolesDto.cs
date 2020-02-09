using ObjectConfig.Data;
using ObjectConfig.Features.Users;

namespace ObjectConfig.Features.Environments.Update
{
    public class EnvironmentUserRolesDto
       : IUserAcessLevel<UsersEnvironments.Role>
    {
        private EnvironmentUserRolesDto()
        { }

        public EnvironmentUserRolesDto(int userId, UsersEnvironments.Role role, EntityOperation operation)
        {
            UserId = userId;
            Role = role;
            Operation = operation;
        }

        public int UserId { get; }
        public UsersEnvironments.Role Role { get; }
        public EntityOperation Operation { get; }
    }
}
