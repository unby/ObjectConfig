using System.Net;
using System.Threading.Tasks;
using ObjectConfig.Data;
using ObjectConfig.Features.Configs;
using UnitTests.Data;
using Xunit;

namespace UnitTests.Controllers
{
    public partial class ConfigAdminsRole
    {
          [Fact]
        public async Task It_should_update_field()
        {
            var testEntityV1 = new TestEntity();
            testEntityV1.ThirdEntity.EntityName = nameof(testEntityV1);

            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();

            var result = await client.PostAsync(
                    $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/forUpdate",
                    testEntityV1.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            testEntityV1.ThirdEntity.EntityName = "updatedName";

            var ver1Json = await client.PatchAsync(
                $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/forUpdate",
                testEntityV1.Serialize());
            Assert.Equal(HttpStatusCode.OK, ver1Json.StatusCode);

            var configFound =
                await client.GetAsync(
                    $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/forUpdate/json");
            Assert.Equal(HttpStatusCode.OK, configFound.StatusCode);

            var ver1Data = configFound.Deserialize<TestEntity>();
            Assert.Equal(ver1Data.ThirdEntity.EntityName, testEntityV1.ThirdEntity.EntityName);

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
