using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using ObjectConfig.Data;
using ObjectConfig.Features.Configs;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnitTests.Data;
using UnitTests.Mock;
using Xunit;
using Xunit.Abstractions;
using Environment = ObjectConfig.Data.Environment;

namespace UnitTests.Controllers
{
    public partial class ConfigAdminsRole : ServerTestBase
    {
        private Environment _env2;
        private Environment _env1;

        public ConfigAdminsRole(ITestOutputHelper output)
            : base(output)
        {
        }

        protected override void SeedData(ObjectConfigContext context, MockUserProvider userProvider)
        {
            Application app1 = DataSeed.Application1;
            Application app2 = DataSeed.Application2;
            User viewer = DataSeed.UserViewer1;
            User admin = DataSeed.UserAdmin1;

            context.UsersApplications.Add(new UsersApplications(viewer, app1, ApplicationRole.Viewer));
            context.UsersApplications.Add(new UsersApplications(userProvider.User, app1,
                ApplicationRole.Administrator));
            context.UsersApplications.Add(new UsersApplications(userProvider.User, app2, ApplicationRole.Viewer));

            _env1 = DataSeed.Environment1(app1);
            _env2 = DataSeed.Environment2(app1);

            context.UsersEnvironments.Add(new UsersEnvironments(admin, _env1, EnvironmentRole.Editor));
            context.UsersEnvironments.Add(new UsersEnvironments(admin, _env2, EnvironmentRole.Editor));

            context.UsersEnvironments.Add(new UsersEnvironments(userProvider.User, _env1, EnvironmentRole.TargetEditor));
            context.UsersEnvironments.Add(new UsersEnvironments(userProvider.User, _env2, EnvironmentRole.Editor));

            _env1.CreateConfig("conf1");
            _env1.CreateConfig("conf2");
            _env1.CreateConfig("conf3");
        }

        [Fact]
        public async Task It_should_get_all()
        {
            using TestServer server = TestServer(UserRole.Administrator);
            using HttpClient client = server.CreateHttpClient();
            HttpResponseMessage result =
                await client.GetAsync($"features/application/{_env1.Application.Code}/environment/{_env1.Code}/configs");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            List<Config> envs = result.Deserialize<List<Config>>();
            envs.Should().HaveCount(3);
        }

        [Fact]
        public async Task It_should_notfound_any()
        {
            using TestServer server = TestServer(UserRole.Administrator);
            using HttpClient client = server.CreateHttpClient();
            HttpResponseMessage result =
                await client.GetAsync($"features/application/{_env1.Application.Code}/environment/notfound/configs");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task It_should_notfound()
        {
            using TestServer server = TestServer(UserRole.Administrator);
            using HttpClient client = server.CreateHttpClient();
            HttpResponseMessage result =
                await client.GetAsync(
                    $"features/application/{_env1.Application.Code}/environment/{_env1.Code}/config/notfound");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task It_should_found()
        {
            using TestServer server = TestServer(UserRole.Administrator);
            using HttpClient client = server.CreateHttpClient();
            HttpResponseMessage result =
                await client.GetAsync(
                    $"features/application/{_env1.Application.Code}/environment/{_env1.Code}/config/conf1");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            ConfigDto env = result.Deserialize<ConfigDto>();
            Assert.Equal("conf1", env.Code);
        }

        [Fact]
        public async Task It_should_create()
        {
            TestEntity testEntity = new TestEntity();
            using TestServer server = TestServer(UserRole.Administrator);
            using HttpClient client = server.CreateHttpClient();
            HttpResponseMessage result = await client.PostAsync($"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/createtest", testEntity.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            HttpResponseMessage configFound =
                 await client.GetAsync(
                     $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/createtest");
            Assert.Equal(HttpStatusCode.OK, configFound.StatusCode);

            ConfigDto env = result.Deserialize<ConfigDto>();
            Assert.Equal("createtest", env.Code);
        }

        [Fact]
        public async Task It_should_create_with_version_and_without()
        {
            TestEntity testEntityV1 = new TestEntity();
            testEntityV1.ThirdEntity.EntityName = nameof(testEntityV1);

            using TestServer server = TestServer(UserRole.Administrator);
            using HttpClient client = server.CreateHttpClient();

            HttpResponseMessage result = await client.PostAsync($"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/createver", testEntityV1.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            TestEntity testEntityV2 = new TestEntity();
            testEntityV2.ThirdEntity.EntityName = nameof(testEntityV2);
            result = await client.PostAsync($"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/createver?versionFrom=2.0.0", testEntityV2.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            TestEntity testEntityV3 = new TestEntity();
            testEntityV3.ThirdEntity.EntityName = nameof(testEntityV3);
            result = await client.PostAsync($"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/createver?versionFrom=0.5.0", testEntityV3.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            HttpResponseMessage configVerFound =
                 await client.GetAsync(
                     $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/createver?versionFrom=2.0.0");
            Assert.Equal(HttpStatusCode.OK, configVerFound.StatusCode);

            ConfigDto confV2Devenition = configVerFound.Deserialize<ConfigDto>();
            Assert.Equal("createver", confV2Devenition.Code);
            Assert.Equal("2.0.0", confV2Devenition.VersionFrom);

            HttpResponseMessage configOriginalFound =
                await client.GetAsync(
                    $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/createver");
            Assert.Equal(HttpStatusCode.OK, configOriginalFound.StatusCode);

            ConfigDto confV1Devenition = configOriginalFound.Deserialize<ConfigDto>();
            Assert.Equal(confV1Devenition.Code, confV2Devenition.Code);
            Assert.NotEqual(confV1Devenition.VersionFrom, confV2Devenition.VersionFrom);
            Assert.Null(confV2Devenition.VersionTo);
            Assert.NotNull(confV1Devenition.VersionTo);

            HttpResponseMessage ver1Json = await client.GetAsync($"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/createver/json");
            Assert.Equal(HttpStatusCode.OK, ver1Json.StatusCode);

            TestEntity ver1Data = ver1Json.Deserialize<TestEntity>();
            Assert.Equal(ver1Data.ThirdEntity.EntityName, testEntityV1.ThirdEntity.EntityName);

            HttpResponseMessage ver2Json = await client.GetAsync($"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/createver/json?versionFrom=2.0.0");
            Assert.Equal(HttpStatusCode.OK, ver1Json.StatusCode);

            TestEntity ver2Data = ver2Json.Deserialize<TestEntity>();
            Assert.Equal(ver2Data.ThirdEntity.EntityName, testEntityV2.ThirdEntity.EntityName);

            HttpResponseMessage ver3Json = await client.GetAsync($"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/createver/json?versionFrom=0.5.0");
            Assert.Equal(HttpStatusCode.OK, ver3Json.StatusCode);

            TestEntity ver3Data = ver3Json.Deserialize<TestEntity>();
            Assert.Equal(ver3Data.ThirdEntity.EntityName, testEntityV3.ThirdEntity.EntityName);
        }

        [Fact]
        public async Task It_should_create_with_version()
        {
            TestEntity testEntity = new TestEntity();
            using TestServer server = TestServer(UserRole.Administrator);
            using HttpClient client = server.CreateHttpClient();
            HttpResponseMessage result = await client.PostAsync($"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/createver?versionFrom=2.0.0", testEntity.Serialize());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            HttpResponseMessage configFound =
                 await client.GetAsync(
                     $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/createver?versionFrom=2.0.0");

            Assert.Equal(HttpStatusCode.OK, configFound.StatusCode);
            ConfigDto confDevenition = result.Deserialize<ConfigDto>();
            Assert.Equal("createver", confDevenition.Code);
            Assert.Equal("2.0.0", confDevenition.VersionFrom);
        }

        [Fact]
        public async Task It_should_forbiden_create()
        {
            TestEntity testEntity = new TestEntity();
            using TestServer server = TestServer(UserRole.Administrator);
            using HttpClient client = server.CreateHttpClient();
            HttpResponseMessage result = await client.PostAsync($"features/application/{_env1.Application.Code}/environment/{_env1.Code}/config/createtest", testEntity.Serialize());

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);

            HttpResponseMessage configFound =
                 await client.GetAsync(
                     $"features/application/{_env1.Application.Code}/environment/{_env1.Code}/config/createtest");
            Assert.Equal(HttpStatusCode.NotFound, configFound.StatusCode);
        }

        [Fact]
        public async Task It_should_not_create()
        {
            TestEntity testEntity = new TestEntity();
            testEntity.ThirdEntity.EntityName = nameof(testEntity);
            using TestServer server = TestServer(UserRole.Administrator);
            using HttpClient client = server.CreateHttpClient();

            HttpResponseMessage result =
                await client.PostAsync(
                    $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/updatetest", testEntity.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            TestEntity updateEntity = new TestEntity();
            updateEntity.ThirdEntity.EntityName = nameof(updateEntity);
            HttpResponseMessage patchResult =
                await client.PostAsync(
                    $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/updatetest", updateEntity.Serialize());
            Assert.Equal(HttpStatusCode.Conflict, patchResult.StatusCode);
        }
    }
}
