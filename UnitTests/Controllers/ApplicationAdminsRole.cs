using ObjectConfig.Data;
using ObjectConfig.Features.Applictaions;
using ObjectConfig.Features.Applictaions.Create;
using ObjectConfig.Features.Applictaions.Update;
using System;
using System.Net;
using System.Threading.Tasks;
using UnitTests.Data;
using UnitTests.Mock;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class ApplicationAdminsRole : ServerTestBase
    {
        public ApplicationAdminsRole(ITestOutputHelper output)
            : base(output)
        {
        }

        protected override void SeedData(ObjectConfigContext context, MockUserProvider userProvider)
        {
            Application app1 = DataSeed.Application1;
            Application app2 = DataSeed.Application2;
            ObjectConfig.Data.User viewer = DataSeed.UserViewer1;
            ObjectConfig.Data.User admin = DataSeed.UserAdmin1;
            context.UsersApplications.Add(new UsersApplications(viewer, app1, ApplicationRole.Viewer));
            context.UsersApplications.Add(new UsersApplications(admin, app2, ApplicationRole.Administrator));
            context.UsersApplications.Add(new UsersApplications(userProvider.User, app1, ApplicationRole.Administrator));
        }

        [Fact]
        public async Task It_should_create()
        {
            CreateApplicationDto testApp = new CreateApplicationDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString() };
            using Microsoft.AspNetCore.TestHost.TestServer server = TestServer(UserRole.Administrator);
            using System.Net.Http.HttpClient client = server.CreateHttpClient();
            System.Net.Http.HttpResponseMessage result = await client.PostAsync("feature/application", testApp.Serialize());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task It_should_not_update_if_user_forbiden()
        {
            Application testApp = DataSeed.Application2;
            using Microsoft.AspNetCore.TestHost.TestServer server = TestServer(UserRole.Administrator);
            using System.Net.Http.HttpClient client = server.CreateHttpClient();

            UpdateApplicationDto updtestApp = new UpdateApplicationDto()
            {
                ApplicationDefinition = new DefinitionDto()
                {
                    Description = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                }
            };

            System.Net.Http.HttpResponseMessage result = await client.PatchAsync($"feature/application/{testApp.Code}/update", updtestApp.Serialize());
            Log.WriteLine(result.Content.ReadAsStringAsync().Result);
            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Fact]
        public async Task It_should_Forbidden()
        {
            CreateApplicationDto testApp = new CreateApplicationDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString() };
            using Microsoft.AspNetCore.TestHost.TestServer server = TestServer(UserRole.Administrator);
            using System.Net.Http.HttpClient client = server.CreateHttpClient();
            System.Net.Http.HttpResponseMessage result = await client.GetAsync("feature/application/notcode");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task It_should_update()
        {
            Application testApp = DataSeed.Application1;
            using Microsoft.AspNetCore.TestHost.TestServer server = TestServer(UserRole.Administrator);
            using System.Net.Http.HttpClient client = server.CreateHttpClient();

            UpdateApplicationDto updtestApp = new UpdateApplicationDto()
            {
                ApplicationDefinition = new DefinitionDto()
                {
                    Description = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                }
            };

            System.Net.Http.HttpResponseMessage result = await client.PatchAsync($"feature/application/{testApp.Code}/update", updtestApp.Serialize());
            Log.WriteLine(result.Content.ReadAsStringAsync().Result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            ApplicationDTO updateDto = result.Deserialize<ApplicationDTO>();

            Assert.Equal(updtestApp.ApplicationDefinition.Description, updateDto.Description);
            Assert.Equal(updtestApp.ApplicationDefinition.Name, updateDto.Name);
        }
    }
}
