using ObjectConfig.Data;
using ObjectConfig.Features.Applictaions;
using ObjectConfig.Features.Applictaions.Create;
using ObjectConfig.Features.Applictaions.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnitTests.Data;
using UnitTests.Mock;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class ApplicationViewersRole : ServerTestBase
    {
        public ApplicationViewersRole(ITestOutputHelper output) : base(output)
        {
        }

        protected override void SeedData(ObjectConfigContext context, MockUserProvider userProvider)
        {
            var app1 = DataSeed.Application1;
            var app2 = DataSeed.Application2;
            var app3 = DataSeed.Application3;
            var admin = DataSeed.UserAdmin1;
            context.UsersApplications.Add(new UsersApplications(admin, app2, ApplicationRole.Administrator));
            context.UsersApplications.Add(new UsersApplications(admin, app1, ApplicationRole.Administrator));
            context.UsersApplications.Add(new UsersApplications(userProvider.User, app1, ApplicationRole.Viewer));
            context.UsersApplications.Add(new UsersApplications(userProvider.User, app3, ApplicationRole.Administrator));
        }

        [Fact]
        public async Task It_should_create()
        {
            var testApp = new CreateApplicationDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString() };
            using var server = TestServer(UserRole.Viewer);
            using var client = server.CreateHttpClient();
            var result = await client.PostAsync("feature/application", testApp.Serialize());

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Fact]
        public async Task It_should_not_update_if_user_forbiden()
        {
            var testApp = DataSeed.Application2;
            using var server = TestServer(UserRole.Viewer);
            using var client = server.CreateHttpClient();

            var updtestApp = new UpdateApplicationDto()
            {
                ApplicationDefinition = new DefinitionDto()
                {
                    Description = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                }
            };

            var result = await client.PatchAsync($"feature/application/{testApp.Code}/update", updtestApp.Serialize());
            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Fact]
        public async Task It_should_NotFound()
        {
            using var server = TestServer(UserRole.Viewer);
            using var client = server.CreateHttpClient();

            var result = await client.GetAsync($"feature/application/notExists");
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task It_should_found()
        {
            using var server = TestServer(UserRole.Viewer);
            using var client = server.CreateHttpClient();

            var result = await client.GetAsync($"feature/application");
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var apps = result.Deserialize<List<ApplicationDTO>>();
            Assert.Equal(2, apps.Count());
        }

        [Fact]
        public async Task It_should_forbiden()
        {
            using var server = TestServer(UserRole.Viewer);
            using var client = server.CreateHttpClient();

            var result = await client.GetAsync($"feature/application/{DataSeed.Application2.Code}");
            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }
    }
}
