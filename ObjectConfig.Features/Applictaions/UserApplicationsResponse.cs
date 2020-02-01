#nullable disable
using ObjectConfig.Data;
using System.Collections.Generic;
using System.Linq;

namespace ObjectConfig.Features.Applictaions
{
    public class UserApplicationsResponse
    {
        public UserApplicationsResponse(List<UsersApplications> usersApplications)
        {
            Applications = usersApplications.Select(s => new ApplicationDTO(s)).ToList();
        }

        public List<ApplicationDTO> Applications { get; }
    }

}
