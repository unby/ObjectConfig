#nullable disable
using ObjectConfig.Data;
using System.Collections.Generic;
using System.Linq;

namespace ObjectConfig.Features.Applictaions
{
    public class ApplicationDTO
    {
        private ApplicationDTO()
        {
        }

        public ApplicationDTO(int applicationId, string name, string code, string? description, UsersApplications.Role role)
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

        public UsersApplications.Role Role { get; protected set; }
    }

    public class UserApplicationsResponse
    {
        public UserApplicationsResponse(List<UsersApplications> usersApplications)
        {
            Applications = usersApplications.Select(s => new ApplicationDTO(s)).ToList();
        }

        public List<ApplicationDTO> Applications { get; }
    }

}
