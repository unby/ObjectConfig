using ObjectConfig.Data;
using ObjectConfig.Features.Applictaions;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class Application : ServerTestBase
    {
        public Application(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CreateApplication()
        {
            var testApp = new ApplicationDTO() { Code=Guid.NewGuid().ToString(),Name=Guid.NewGuid().ToString() };
            using var server = TestServer(User.Role.Administrator);
            using var client = server.CreateHttpClient();
            var result = await client.PostAsync("feature/application", testApp.Serialize());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
