using FluentAssertions;
using ObjectConfig.Data;
using ObjectConfig.Features.Environments;
using ObjectConfig.Features.Environments.Create;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnitTests.Data;
using UnitTests.Mock;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class EnvironmentAdminsRole : ServerTestBase
    {
        private ObjectConfig.Data.Environment _app2env1;

        public ObjectConfig.Data.Environment ForUpdateEnv { get; private set; }

        public EnvironmentAdminsRole(ITestOutputHelper output) : base(output)
        {
        }

        protected override void SeedData(ObjectConfigContext context, MockUserProvider userProvider)
        {
            var app1 = DataSeed.Application1;
            var app2 = DataSeed.Application2;
            var viewer = DataSeed.UserViewer1;
            var admin = DataSeed.UserAdmin1;

            context.UsersApplications.Add(new UsersApplications(viewer, app1, UsersApplications.Role.Viewer));
            context.UsersApplications.Add(new UsersApplications(userProvider.User, app1, UsersApplications.Role.Administrator));
            context.UsersApplications.Add(new UsersApplications(userProvider.User, app2, UsersApplications.Role.Viewer));

            var env1 = DataSeed.Environment1(app1);
            var env2 = DataSeed.Environment2(app1);
            ForUpdateEnv = DataSeed.Environment3(app1);

            context.UsersEnvironments.Add(new UsersEnvironments(admin, env1, UsersEnvironments.Role.Editor));
            context.UsersEnvironments.Add(new UsersEnvironments(admin, env2, UsersEnvironments.Role.Editor));
            context.UsersEnvironments.Add(new UsersEnvironments(admin, ForUpdateEnv, UsersEnvironments.Role.Editor));

            context.UsersEnvironments.Add(new UsersEnvironments(userProvider.User, env1, UsersEnvironments.Role.Viewer));
            context.UsersEnvironments.Add(new UsersEnvironments(userProvider.User, env2, UsersEnvironments.Role.Viewer));
            context.UsersEnvironments.Add(new UsersEnvironments(userProvider.User, ForUpdateEnv, UsersEnvironments.Role.Viewer));

            _app2env1 = DataSeed.Environment1(app2);
            context.UsersEnvironments.Add(new UsersEnvironments(admin, _app2env1, UsersEnvironments.Role.Editor));
        }

        [Fact]
        public async Task It_should_get_all()
        {
            using var server = TestServer(User.Role.Administrator);
            using var client = server.CreateHttpClient();
            var result = await client.GetAsync($"features/application/{DataSeed.Application1.Code}/environments");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var envs = result.Deserialize<List<EnvironmentDto>>();
            envs.Should().HaveCount(3);
        }

        [Fact]
        public async Task It_should_notfound()
        {
            using var server = TestServer(User.Role.Administrator);
            using var client = server.CreateHttpClient();
            var result = await client.GetAsync($"features/application/{_app2env1.Application.Code}/environment/{_app2env1.Code}");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task It_should_found()
        {
            using var server = TestServer(User.Role.Administrator);
            using var client = server.CreateHttpClient();
            var result = await client.GetAsync($"features/application/{DataSeed.Application1.Code}/environment/Environment2");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Log.WriteLine(result.Content.ReadAsStringAsync().Result);
            var env = result.Deserialize<EnvironmentDto>();
            Assert.Equal("Environment2", env.Code);
        }

        [Fact]
        public async Task It_should_create()
        {
            var testEnv = new CreateEnvironmentDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString(), Description = Guid.NewGuid().ToString() };
            using var server = TestServer(User.Role.Administrator);
            using var client = server.CreateHttpClient();
            var result = await client.PostAsync($"features/application/{DataSeed.Application1.Code}/environment/", testEnv.Serialize());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Log.WriteLine(result.Content.ReadAsStringAsync().Result);
            var env = result.Deserialize<EnvironmentDto>();
            Assert.Equal(testEnv.Code, env.Code);
            Assert.Equal(testEnv.Description, env.Description);
            Assert.Equal(testEnv.Name, env.Name);

            result = await client.GetAsync($"features/application/{DataSeed.Application1.Code}/environment/{testEnv.Code}");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            env = result.Deserialize<EnvironmentDto>();
            Assert.Equal(testEnv.Code, env.Code);
            Assert.Equal(testEnv.Description, env.Description);
            Assert.Equal(testEnv.Name, env.Name);
        }

        [Fact]
        public async Task It_should_forbiden_create()
        {
            var testEnv = new CreateEnvironmentDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString(), Description = Guid.NewGuid().ToString() };
            using var server = TestServer(User.Role.Administrator);
            using var client = server.CreateHttpClient();
            var result = await client.PostAsync($"features/application/{DataSeed.Application2.Code}/environment/", testEnv.Serialize());

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);

            result = await client.GetAsync($"features/application/{DataSeed.Application2.Code}/environment/{testEnv.Code}");
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task It_should_update()
        {
            var testEnv = new CreateEnvironmentDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString(), Description = Guid.NewGuid().ToString() };
            using var server = TestServer(User.Role.Administrator);
            using var client = server.CreateHttpClient();
            var result = await client.PatchAsync($"features/application/{ForUpdateEnv.Application.Code}/environment/{ForUpdateEnv.Code}", testEnv.Serialize());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Log.WriteLine(result.Content.ReadAsStringAsync().Result);
            var env = result.Deserialize<EnvironmentDto>();
            Assert.Equal(testEnv.Code, env.Code);
            Assert.Equal(testEnv.Description, env.Description);
            Assert.Equal(testEnv.Name, env.Name);

            result = await client.GetAsync($"features/application/{DataSeed.Application1.Code}/environment/{testEnv.Code}");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            env = result.Deserialize<EnvironmentDto>();
            Assert.Equal(testEnv.Code, env.Code);
            Assert.Equal(testEnv.Description, env.Description);
            Assert.Equal(testEnv.Name, env.Name);
        }

        [Fact]
        public async Task It_should_forbiden_update()
        {
            var testEnv = new CreateEnvironmentDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString(), Description = Guid.NewGuid().ToString() };
            using var server = TestServer(User.Role.Administrator);
            using var client = server.CreateHttpClient();
            var result = await client.PostAsync($"features/application/{DataSeed.Application2.Code}/environment/", testEnv.Serialize());

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);

            result = await client.GetAsync($"features/application/{DataSeed.Application2.Code}/environment/{testEnv.Code}");
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
