using FluentAssertions;
using ObjectConfig.Data;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ObjectConfig.Features.Configs;
using UnitTests.Data;
using UnitTests.Mock;
using Xunit;
using Xunit.Abstractions;
using Environment = ObjectConfig.Data.Environment;

namespace UnitTests.Controllers
{
    public class ConfigAdminsRole : ServerTestBase
    {
        private Environment env2;
        private Environment env1;


        public ConfigAdminsRole(ITestOutputHelper output) : base(output)
        {
        }

        protected override void SeedData(ObjectConfigContext context, MockUserProvider userProvider)
        {
            var app1 = DataSeed.Application1;
            var app2 = DataSeed.Application2;
            var viewer = DataSeed.UserViewer1;
            var admin = DataSeed.UserAdmin1;

            context.UsersApplications.Add(new UsersApplications(viewer, app1, ApplicationRole.Viewer));
            context.UsersApplications.Add(new UsersApplications(userProvider.User, app1,
                ApplicationRole.Administrator));
            context.UsersApplications.Add(new UsersApplications(userProvider.User, app2, ApplicationRole.Viewer));

            env1 = DataSeed.Environment1(app1);
            env2 = DataSeed.Environment2(app1);

            context.UsersEnvironments.Add(new UsersEnvironments(admin, env1, EnvironmentRole.Editor));
            context.UsersEnvironments.Add(new UsersEnvironments(admin, env2, EnvironmentRole.Editor));


            context.UsersEnvironments.Add(new UsersEnvironments(userProvider.User, env1, EnvironmentRole.TargetEditor));
            context.UsersEnvironments.Add(new UsersEnvironments(userProvider.User, env2, EnvironmentRole.Editor));

            env1.CreateConfig("conf1");
            env1.CreateConfig("conf2");
            env1.CreateConfig("conf3");
        }

        [Fact]
        public async Task It_should_get_all()
        {
            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();
            var result =
                await client.GetAsync($"features/application/{env1.Application.Code}/environment/{env1.Code}/configs");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var envs = result.Deserialize<List<Config>>();
            envs.Should().HaveCount(3);
        }

        [Fact]
        public async Task It_should_notfound_any()
        {
            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();
            var result =
                await client.GetAsync($"features/application/{env1.Application.Code}/environment/notfound/configs");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task It_should_notfound()
        {
            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();
            var result =
                await client.GetAsync(
                    $"features/application/{env1.Application.Code}/environment/{env1.Code}/config/notfound");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task It_should_found()
        {
            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();
            var result =
                await client.GetAsync(
                    $"features/application/{env1.Application.Code}/environment/{env1.Code}/config/conf1");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var env = result.Deserialize<ConfigDto>();
            Assert.Equal("conf1", env.Code);
        }


        [Fact]
        public async Task It_should_create()
        {
            var testEntity = new TestEntity();
            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();
            var result = await client.PostAsync($"features/application/{env2.Application.Code}/environment/{env2.Code}/config/createtest", testEntity.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            
            var configFound =
                 await client.GetAsync(
                     $"features/application/{env2.Application.Code}/environment/{env2.Code}/config/createtest");
            Assert.Equal(HttpStatusCode.OK, configFound.StatusCode);
            
            var env = result.Deserialize<ConfigDto>();
            Assert.Equal("createtest", env.Code);
        }

        [Fact]
        public async Task It_should_create_with_version_and_without()
        {
            var testEntity = new TestEntity();
            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();

            var result = await client.PostAsync($"features/application/{env2.Application.Code}/environment/{env2.Code}/config/createver", testEntity.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            result = await client.PostAsync($"features/application/{env2.Application.Code}/environment/{env2.Code}/config/createver?versionFrom=2.0.0", testEntity.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            
            var configVerFound =
                 await client.GetAsync(
                     $"features/application/{env2.Application.Code}/environment/{env2.Code}/config/createver?versionFrom=2.0.0");
            Assert.Equal(HttpStatusCode.OK, configVerFound.StatusCode);

            var confV2Devenition = configVerFound.Deserialize<ConfigDto>();
            Assert.Equal("createver", confV2Devenition.Code);
            Assert.Equal("2.0.0", confV2Devenition.VersionFrom);

            var configOriginalFound =
                await client.GetAsync(
                    $"features/application/{env2.Application.Code}/environment/{env2.Code}/config/createver");
            Assert.Equal(HttpStatusCode.OK, configOriginalFound.StatusCode);

            var confV1Devenition = configOriginalFound.Deserialize<ConfigDto>();
            Assert.Equal(confV1Devenition.Code, confV2Devenition.Code);
            Assert.NotEqual(confV1Devenition.VersionFrom, confV2Devenition.VersionFrom);
            Assert.Null(confV2Devenition.VersionTo);
            Assert.NotNull(confV1Devenition.VersionTo);
        }


        [Fact]
        public async Task It_should_create_with_version()
        {
            var testEntity = new TestEntity();
            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();
            var result = await client.PostAsync($"features/application/{env2.Application.Code}/environment/{env2.Code}/config/createver?versionFrom=2.0.0", testEntity.Serialize());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var configFound =
                 await client.GetAsync(
                     $"features/application/{env2.Application.Code}/environment/{env2.Code}/config/createver?versionFrom=2.0.0");

            Assert.Equal(HttpStatusCode.OK, configFound.StatusCode);
            var confDevenition = result.Deserialize<ConfigDto>();
            Assert.Equal("createver", confDevenition.Code);
            Assert.Equal("2.0.0", confDevenition.VersionFrom);
        }

        [Fact]
        public async Task It_should_forbiden_create()
        {
            var testEntity = new TestEntity();
            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();
            var result = await client.PostAsync($"features/application/{env1.Application.Code}/environment/{env1.Code}/config/createtest", testEntity.Serialize());

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);

            var configFound =
                 await client.GetAsync(
                     $"features/application/{env1.Application.Code}/environment/{env1.Code}/config/createtest");
            Assert.Equal(HttpStatusCode.NotFound, configFound.StatusCode);
        }

        [Fact]
        public async Task It_should_not_create()
        {
            var testEntity = new TestEntity();
            testEntity.ThirdEntity.EntityName = nameof(testEntity);
            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();

            var result = 
                await client.PostAsync(
                    $"features/application/{env2.Application.Code}/environment/{env2.Code}/config/updatetest", testEntity.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var updateEntity = new TestEntity();
            updateEntity.ThirdEntity.EntityName = nameof(updateEntity);
            var patchResult = 
                await client.PostAsync(
                    $"features/application/{env2.Application.Code}/environment/{env2.Code}/config/updatetest", updateEntity.Serialize());
            Assert.Equal(HttpStatusCode.Conflict, patchResult.StatusCode);
        }
        /*
        [Fact]
        public async Task It_should_update()
        {
            var testEntity = new TestEntity();
            testEntity.ThirdEntity.EntityName = nameof(testEntity);
            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();

            var result =
                await client.PostAsync(
                    $"features/application/{env2.Application.Code}/environment/{env2.Code}/config/updatetest", testEntity.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var updateEntity = new TestEntity();
            updateEntity.ThirdEntity.EntityName = nameof(updateEntity);
            var patchResult =
                await client.PostAsync(
                    $"features/application/{env2.Application.Code}/environment/{env2.Code}/config/updatetest", updateEntity.Serialize());
            Assert.Equal(HttpStatusCode.OK, patchResult.StatusCode);

            var configFound =
                 await client.GetAsync(
                     $"features/application/{env2.Application.Code}/environment/{env2.Code}/config/updatetest");
            Assert.Equal(HttpStatusCode.OK, configFound.StatusCode);

            var env = configFound.Deserialize<ConfigDto>();
            Assert.Equal("updatetest", env.Code);
         }
        
         [Fact]
         public async Task It_should_forbiden_updates()
         {
             var updtestEnv = new UpdateEnvironmentDto()
             {
                 Definition = new ObjectConfig.Features.Applictaions.Update.DefinitionDto()
                 {
                     Description = Guid.NewGuid().ToString(),
                     Name = Guid.NewGuid().ToString()
                 }
             };

             using var server = TestServer(UserRole.Administrator);
             using var client = server.CreateHttpClient();
             var result = await client.PatchAsync($"features/application/notfound/environment/{ForUpdateEnv.Code}", updtestEnv.Serialize());

             Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

         }*/
    }
}
