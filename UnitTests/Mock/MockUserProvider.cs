using ObjectConfig.Data;
using ObjectConfig.Features.Users;

namespace UnitTests.Mock
{
    public class MockUserProvider : IUserProvider
    {
        private readonly UserDto _user;

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
