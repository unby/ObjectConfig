using ObjectConfig.Data;
using ObjectConfig.Features.Users;

namespace ObjectConfig.Features.Applictaions.Update
{
    public class ApplicationUserRolesDto
        : IUserAcessLevel<UsersApplications.Role>
    {
        private ApplicationUserRolesDto()
        { }

        public ApplicationUserRolesDto(int userId, UsersApplications.Role role, EntityOperation operation)
        {
            UserId = userId;
            Role = role;
            Operation = operation;
        }

        public int UserId { get; }
        public UsersApplications.Role Role { get; }
        public EntityOperation Operation { get; }
    }
}
