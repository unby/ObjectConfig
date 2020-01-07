using Microsoft.AspNetCore.Http;

namespace ObjectConfig.Features.Users
{
    public class UserProvider : IUserProvider
    {
        public UserProvider(IHttpContextAccessor httpContextAccessor)
        {
        }

        public UserDto GetCurrentUser()
        {
            return null;
        }
    }
}
