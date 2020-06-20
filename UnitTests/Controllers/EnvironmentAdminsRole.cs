using FluentAssertions;
using ObjectConfig.Data;
using ObjectConfig.Features.Environments;
using ObjectConfig.Features.Environments.Create;
using ObjectConfig.Features.Environments.Update;
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
        private ObjectConfig.Data.Environment _app2Env1;

        private ObjectConfig.Data.Environment ForUpdateEnv { get; set; }

        public EnvironmentAdminsRole(ITestOutputHelper output)
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
            context.UsersApplications.Add(new UsersApplications(userProvider.User, app1, ApplicationRole.Administrator));
            context.UsersApplications.Add(new UsersApplications(userProvider.User, app2, ApplicationRole.Viewer));

            ObjectConfig.Data.Environment env1 = DataSeed.Environment1(app1);
            ObjectConfig.Data.Environment env2 = DataSeed.Environment2(app1);
            ForUpdateEnv = DataSeed.Environment3(app1);

            context.UsersEnvironments.Add(new UsersEnvironments(admin, env1, EnvironmentRole.Editor));
            context.UsersEnvironments.Add(new UsersEnvironments(admin, env2, EnvironmentRole.Editor));
            context.UsersEnvironments.Add(new UsersEnvironments(admin, ForUpdateEnv, EnvironmentRole.Editor));

            context.UsersEnvironments.Add(new UsersEnvironments(userProvider.User, env1, EnvironmentRole.TargetEditor));
            context.UsersEnvironments.Add(new UsersEnvironments(userProvider.User, env2, EnvironmentRole.TargetEditor));
            context.UsersEnvironments.Add(new UsersEnvironments(userProvider.User, ForUpdateEnv, EnvironmentRole.TargetEditor));

            _app2Env1 = DataSeed.Environment1(app2);
            context.UsersEnvironments.Add(new UsersEnvironments(admin, _app2Env1, EnvironmentRole.Editor));
        }

        [Fact]
        public async Task It_should_get_all()
        {
            using Microsoft.AspNetCore.TestHost.TestServer server = TestServer(UserRole.Administrator);
            using System.Net.Http.HttpClient client = server.CreateHttpClient();
            System.Net.Http.HttpResponseMessage result = await client.GetAsync($"features/application/{DataSeed.Application1.Code}/environments");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            List<EnvironmentDto> envs = result.Deserialize<List<EnvironmentDto>>();
            envs.Should().HaveCount(3);
        }

        [Fact]
        public async Task It_should_notfound()
        {
            using Microsoft.AspNetCore.TestHost.TestServer server = TestServer(UserRole.Administrator);
            using System.Net.Http.HttpClient client = server.CreateHttpClient();
            System.Net.Http.HttpResponseMessage result = await client.GetAsync($"features/application/{_app2Env1.Application.Code}/environment/{_app2Env1.Code}");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task It_should_found()
        {
            using Microsoft.AspNetCore.TestHost.TestServer server = TestServer(UserRole.Administrator);
            using System.Net.Http.HttpClient client = server.CreateHttpClient();
            System.Net.Http.HttpResponseMessage result = await client.GetAsync($"features/application/{DataSeed.Application1.Code}/environment/Environment2");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Log.WriteLine(result.Content.ReadAsStringAsync().Result);
            EnvironmentDto env = result.Deserialize<EnvironmentDto>();
            Assert.Equal("Environment2", env.Code);
        }

        [Fact]
        public async Task It_should_create()
        {
            CreateEnvironmentDto testEnv = new CreateEnvironmentDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString(), Description = Guid.NewGuid().ToString() };
            using Microsoft.AspNetCore.TestHost.TestServer server = TestServer(UserRole.Administrator);
            using System.Net.Http.HttpClient client = server.CreateHttpClient();
            System.Net.Http.HttpResponseMessage result = await client.PostAsync($"features/application/{DataSeed.Application1.Code}/environment/", testEnv.Serialize());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Log.WriteLine(result.Content.ReadAsStringAsync().Result);
            EnvironmentDto env = result.Deserialize<EnvironmentDto>();
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
            CreateEnvironmentDto testEnv = new CreateEnvironmentDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString(), Description = Guid.NewGuid().ToString() };
            using Microsoft.AspNetCore.TestHost.TestServer server = TestServer(UserRole.Administrator);
            using System.Net.Http.HttpClient client = server.CreateHttpClient();
            System.Net.Http.HttpResponseMessage result = await client.PostAsync($"features/application/{DataSeed.Application2.Code}/environment/", testEnv.Serialize());

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);

            result = await client.GetAsync($"features/application/{DataSeed.Application2.Code}/environment/{testEnv.Code}");
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task It_should_update()
        {
            UpdateEnvironmentDto updtestEnv = new UpdateEnvironmentDto()
            {
                Definition = new ObjectConfig.Features.Applictaions.Update.DefinitionDto()
                {
                    Description = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                }
            };

            using Microsoft.AspNetCore.TestHost.TestServer server = TestServer(UserRole.Administrator);
            using System.Net.Http.HttpClient client = server.CreateHttpClient();
            System.Net.Http.HttpResponseMessage result = await client.PatchAsync($"features/application/{ForUpdateEnv.Application.Code}/environment/{ForUpdateEnv.Code}", updtestEnv.Serialize());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            EnvironmentDto env = result.Deserialize<EnvironmentDto>();
            Assert.Equal(updtestEnv.Definition.Description, env.Description);
            Assert.Equal(updtestEnv.Definition.Name, env.Name);

            result = await client.GetAsync($"features/application/{ForUpdateEnv.Application.Code}/environment/{ForUpdateEnv.Code}");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            env = result.Deserialize<EnvironmentDto>();
            Assert.Equal(updtestEnv.Definition.Description, env.Description);
            Assert.Equal(updtestEnv.Definition.Name, env.Name);
        }

        [Fact]
        public async Task It_should_forbiden_updates()
        {
            UpdateEnvironmentDto updtestEnv = new UpdateEnvironmentDto()
            {
                Definition = new ObjectConfig.Features.Applictaions.Update.DefinitionDto()
                {
                    Description = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                }
            };

            using Microsoft.AspNetCore.TestHost.TestServer server = TestServer(UserRole.Administrator);
            using System.Net.Http.HttpClient client = server.CreateHttpClient();
            System.Net.Http.HttpResponseMessage result = await client.PatchAsync($"features/application/notfound/environment/{ForUpdateEnv.Code}", updtestEnv.Serialize());

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
