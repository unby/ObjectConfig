using Microsoft.AspNetCore.Http;
using System;

namespace ObjectConfig.Features.Users
{
    public class UserProvider : IUserProvider
    {
        public UserProvider(IHttpContextAccessor httpContextAccessor)
        {
        }

        public UserDto GetCurrentUser()
        {
            throw new NotImplementedException();
        }
    }
}
