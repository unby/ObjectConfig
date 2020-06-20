using Microsoft.AspNetCore.TestHost;
using ObjectConfig.Data;
using ObjectConfig.Features.Applictaions;
using ObjectConfig.Features.Applictaions.Create;
using ObjectConfig.Features.Applictaions.Update;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnitTests.Data;
using UnitTests.Mock;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class ApplicationGlobalAdminsRole
        : ServerTestBase
    {
        public ApplicationGlobalAdminsRole(ITestOutputHelper output)
            : base(output)
        {
        }

        protected override void SeedData(ObjectConfigContext context, MockUserProvider userProvider)
        {
            Application app1 = DataSeed.Application1;
            ObjectConfig.Data.User viewer = DataSeed.UserViewer1;
            context.UsersApplications.Add(new UsersApplications(viewer, app1, ApplicationRole.Viewer));
        }

        [Fact]
        public async Task It_should_create()
        {
            CreateApplicationDto testApp = new CreateApplicationDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString() };
            using TestServer server = TestServer(UserRole.GlobalAdministrator);
            using HttpClient client = server.CreateHttpClient();
            HttpResponseMessage result = await client.PostAsync("feature/application", testApp.Serialize());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task It_should_FotFound()
        {
            CreateApplicationDto testApp = new CreateApplicationDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString() };
            using TestServer server = TestServer(UserRole.GlobalAdministrator);
            using HttpClient client = server.CreateHttpClient();
            HttpResponseMessage result = await client.GetAsync("feature/application/notcode");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task It_should_update()
        {
            Application testApp = DataSeed.Application1;
            using TestServer server = TestServer(UserRole.GlobalAdministrator);
            using HttpClient client = server.CreateHttpClient();

            UpdateApplicationDto updtestApp = new UpdateApplicationDto()
            {
                ApplicationDefinition = new DefinitionDto()
                {
                    Description = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                }
            };

            HttpResponseMessage result = await client.PatchAsync($"feature/application/{testApp.Code}/update", updtestApp.Serialize());
            Log.WriteLine(result.Content.ReadAsStringAsync().Result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            ApplicationDTO updateDto = result.Deserialize<ApplicationDTO>();

            Assert.Equal(updtestApp.ApplicationDefinition.Description, updateDto.Description);
            Assert.Equal(updtestApp.ApplicationDefinition.Name, updateDto.Name);
        }
    }
}
