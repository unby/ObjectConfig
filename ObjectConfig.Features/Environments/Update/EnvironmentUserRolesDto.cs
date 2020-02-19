using ObjectConfig.Data;
using ObjectConfig.Features.Users;

namespace ObjectConfig.Features.Environments.Update
{
    public class EnvironmentUserRolesDto
       : IUserAcessLevel<EnvironmentRole>
    {
        private EnvironmentUserRolesDto()
        { }

        public EnvironmentUserRolesDto(int userId, EnvironmentRole role, EntityOperation operation)
        {
            UserId = userId;
            Role = role;
            Operation = operation;
        }

        public int UserId { get; }
        public EnvironmentRole Role { get; }
        public EntityOperation Operation { get; }
    }
}
