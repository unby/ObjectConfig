using ObjectConfig.Data;
using ObjectConfig.Features.Users;
using System;

namespace UnitTests.Mock
{
    public class MockUserProvider : IUserProvider
    {
        public readonly UserDto _user;

        public MockUserProvider(UserRole role)
            : this(Guid.NewGuid().ToString(),
                  Guid.NewGuid().ToString(),
                  Guid.NewGuid().ToString() + "@test.test",
                  role)
        {
        }

        public MockUserProvider(string externalId, string displayName, string email, UserRole role)
        {
            _user = new UserDto(-1, externalId, displayName, email, role);
            User = new User(externalId, displayName, email, role);
        }

        public User User { get; }

        public MockUserProvider(UserDto user)
        {
            _user = user;
            User = new User(user.ExternalId, user.DisplayName, user.Email, user.AccessRole);
        }

        public MockUserProvider(User user)
        {
            _user = new UserDto(-1, user.ExternalId, user.DisplayName, user.Email, user.AccessRole);
            User = user;
        }

        public UserDto GetCurrentUser()
        {
            return _user;
        }
    }
}
