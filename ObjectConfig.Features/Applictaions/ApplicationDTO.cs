using ObjectConfig.Data;

namespace ObjectConfig.Features.Applictaions
{
    public class ApplicationDTO
    {
        public int ApplicationId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public UsersApplications.Role Role { get; set; }
    }
}
