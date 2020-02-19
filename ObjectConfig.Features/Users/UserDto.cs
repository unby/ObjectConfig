using ObjectConfig.Data;

#nullable disable
namespace ObjectConfig.Features.Users
{
    public class UserDto
    {
        private UserDto()
        {
        }

        public UserDto(int userId, string externalId, string displayName, string email, UserRole accessRole)
        {
            Email = email;
            AccessRole = accessRole;
            UserId = userId;
            ExternalId = externalId;
            DisplayName = displayName;
        }

        public string Email { get; private set; }

        public UserRole AccessRole { get; private set; }

        public int UserId { get; private set; }

        public string ExternalId { get; private set; }

        public string DisplayName { get; private set; }
    }
}
