using ObjectConfig.Data;

namespace ObjectConfig.Features.Users
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string ExternalId { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public User.Role AccessRole { get; set; }
    }
}
