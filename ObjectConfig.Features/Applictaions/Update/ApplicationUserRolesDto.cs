using ObjectConfig.Data;
using ObjectConfig.Features.Users;

namespace ObjectConfig.Features.Applictaions.Update
{
    public class ApplicationUserRolesDto
        : IUserAcessLevel<ApplicationRole>
    {
        private ApplicationUserRolesDto()
        { }

        public ApplicationUserRolesDto(int userId, ApplicationRole role, EntityOperation operation)
        {
            UserId = userId;
            Role = role;
            Operation = operation;
        }

        public int UserId { get; }
        public ApplicationRole Role { get; }
        public EntityOperation Operation { get; }
    }
}
