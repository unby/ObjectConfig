using ObjectConfig.Data;
using ObjectConfig.Features.Users;
using System.Net;
using System.Threading.Tasks;
using UnitTests.Mock;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class UserTest : ServerTestBase
    {
        public UserTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task It_should_get_user()
        {
            var testUser = new MockUserProvider(UserRole.Viewer);
            using var server = TestServer(testUser);
            using var client = server.CreateHttpClient();
            var result = await client.GetAsync("feature/user");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var responseDto = result.Deserialize<UserDto>();
            Assert.Equal(testUser._user.DisplayName, responseDto.DisplayName);
            Assert.Equal(testUser._user.AccessRole, responseDto.AccessRole);
            Assert.Equal(testUser._user.Email, responseDto.Email);
            Assert.Equal(testUser._user.ExternalId, responseDto.ExternalId);
            Assert.NotEqual(0, responseDto.UserId);
        }
    }
}
