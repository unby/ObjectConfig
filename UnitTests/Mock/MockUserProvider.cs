using ObjectConfig.Data;
using ObjectConfig.Features.Users;
using System;

namespace UnitTests.Mock
{
    public class MockUserProvider : IUserProvider
    {
        public readonly UserDto _user;

        public MockUserProvider(User.Role role)
            : this(Guid.NewGuid().ToString(),
                  Guid.NewGuid().ToString(),
                  Guid.NewGuid().ToString() + "@test.test",
                  role)
        {
        }

        public MockUserProvider(string externalId, string displayName, string email, User.Role role)
        {
            _user = new UserDto()
            {
                ExternalId = externalId,
                DisplayName = displayName,
                AccessRole = role,
                Email = email
            };
        }

        public MockUserProvider(UserDto user)
        {
            _user = user;
        }

        public MockUserProvider(User user)
        {
            _user = new UserDto()
            {
                ExternalId = user.ExternalId,
                DisplayName = user.DisplayName,
                AccessRole = user.AccessRole,
                Email = user.Email
            };
        }

        public UserDto GetCurrentUser()
        {
            return _user;
        }
    }
}
