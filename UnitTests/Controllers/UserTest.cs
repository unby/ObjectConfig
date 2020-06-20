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
        public UserTest(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task It_should_get_user()
        {
            MockUserProvider testUser = new MockUserProvider(UserRole.Viewer);
            using Microsoft.AspNetCore.TestHost.TestServer server = TestServer(testUser);
            using System.Net.Http.HttpClient client = server.CreateHttpClient();
            System.Net.Http.HttpResponseMessage result = await client.GetAsync("feature/user");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            UserDto responseDto = result.Deserialize<UserDto>();
            Assert.Equal(testUser._user.DisplayName, responseDto.DisplayName);
            Assert.Equal(testUser._user.AccessRole, responseDto.AccessRole);
            Assert.Equal(testUser._user.Email, responseDto.Email);
            Assert.Equal(testUser._user.ExternalId, responseDto.ExternalId);
            Assert.NotEqual(0, responseDto.UserId);
        }
    }
}
