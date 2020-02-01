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
        public ApplicationAdminsRole(ITestOutputHelper output) : base(output)
        {
        }

        protected override void SeedData(ObjectConfigContext context, MockUserProvider userProvider)
        {
            var app1 = DataSeed.Application1;
            var app2 = DataSeed.Application2;
            var viewer = DataSeed.UserViewer1;
            var admin = DataSeed.UserAdmin1;
            context.UsersApplications.Add(new UsersApplications(viewer, app1, UsersApplications.Role.Viewer));
            context.UsersApplications.Add(new UsersApplications(admin, app2, UsersApplications.Role.Administrator));
            context.UsersApplications.Add(new UsersApplications(userProvider.User, app1, UsersApplications.Role.Administrator));
        }

        [Fact]
        public async Task It_should_create()
        {
            var testApp = new CreateApplicationDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString() };
            using var server = TestServer(User.Role.Administrator);
            using var client = server.CreateHttpClient();
            var result = await client.PostAsync("feature/application", testApp.Serialize());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public async Task It_should_not_update_if_user_forbiden()
        {
            var testApp = DataSeed.Application2;
            using var server = TestServer(User.Role.Administrator);
            using var client = server.CreateHttpClient();

            var updtestApp = new UpdateApplicationDto()
            {
                ApplicationDefinition = new ApplicationDefinitionDto()
                {
                    Description = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                }
            };

            var result = await client.PatchAsync($"feature/application/{testApp.Code}/update", updtestApp.Serialize());
            Log.WriteLine(result.Content.ReadAsStringAsync().Result);
            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Fact]
        public async Task It_should_Forbidden()
        {
            var testApp = new CreateApplicationDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString() };
            using var server = TestServer(User.Role.Administrator);
            using var client = server.CreateHttpClient();
            var result = await client.GetAsync("feature/application/notcode");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task It_should_update()
        {
            var testApp = DataSeed.Application1;
            using var server = TestServer(User.Role.Administrator);
            using var client = server.CreateHttpClient();

            var updtestApp = new UpdateApplicationDto()
            {
                ApplicationDefinition = new ApplicationDefinitionDto()
                {
                    Description = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                }
            };

            var result = await client.PatchAsync($"feature/application/{testApp.Code}/update", updtestApp.Serialize());
            Log.WriteLine(result.Content.ReadAsStringAsync().Result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var updateDto = result.Deserialize<ApplicationDTO>();

            Assert.Equal(updtestApp.ApplicationDefinition.Description, updateDto.Description);
            Assert.Equal(updtestApp.ApplicationDefinition.Name, updateDto.Name);
        }
    }
}
