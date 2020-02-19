#nullable disable
using ObjectConfig.Data;

namespace ObjectConfig.Features.Applictaions
{
    public class ApplicationDTO
    {
#nullable disable
        private ApplicationDTO()
        {
        }
#nullable enable
        public ApplicationDTO(int applicationId, string name, string code, string? description, ApplicationRole role)
        {
            ApplicationId = applicationId;
            Name = name;
            Code = code;
            Description = description;
            Role = role;
        }

        public ApplicationDTO(UsersApplications usersApplications)
        {
            ApplicationId = usersApplications.ApplicationId;
            Name = usersApplications.Application.Name;
            Code = usersApplications.Application.Code;
            Description = usersApplications.Application.Description;
            Role = usersApplications.AccessRole;
        }

        public int ApplicationId { get; protected set; }

        public string Name { get; protected set; }

        public string Code { get; protected set; }

        public string? Description { get; protected set; }

        public ApplicationRole Role { get; protected set; }
    }
}
