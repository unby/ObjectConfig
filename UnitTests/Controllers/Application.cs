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
        public async Task It_should_get_user()
        {
            using var server = TestServer();
            using var client = server.CreateHttpClient();
            var result = await client.GetAsync("feature/WeatherForecast");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
