#nullable disable
namespace ObjectConfig.Features.Users
{
    public class UserEntityDto
    {
        protected UserEntityDto()
        {
        }

        public UserEntityDto(int userId, string externalId, string displayName)
        {

        }

        public int UserId { get; protected set; }

        public string ExternalId { get; protected set; }

        public string DisplayName { get; protected set; }
    }
}
