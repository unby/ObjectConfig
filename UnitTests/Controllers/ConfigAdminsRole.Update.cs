using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ObjectConfig.Data;
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

        [Fact]
        public async Task It_should_update_array_field()
        {
            var testEntityV1 =
                new Dictionary<string, object> {{"Array", new[] {"str1", "str2"}}, {"EntityName", "array"}};

            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();

            var result = await client.PostAsync(
                $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/arrayUpdate",
                testEntityV1.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var testEntityV2 =
                new Dictionary<string, object> {{"Array", new[] {"stru1", "stru2"}}, {"EntityName", "array2"}};


            var ver1Json = await client.PatchAsync(
                $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/arrayUpdate",
                testEntityV2.Serialize());
            Assert.Equal(HttpStatusCode.OK, ver1Json.StatusCode);

            var configFound =
                await client.GetAsync(
                    $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/arrayUpdate/json");
            Assert.Equal(HttpStatusCode.OK, configFound.StatusCode);

            var ver1Data = configFound.Deserialize<dynamic>();
            Assert.Equal(ver1Data.Array, testEntityV2["Array"]);
            Assert.Equal(ver1Data.EntityName, testEntityV2["EntityName"]);

        }

        [Fact]
        public async Task It_should_update_type_field()
        {
            var testEntityV1 = new Dictionary<string, object> {{"var", 123}, {"EntityName", "DateTimeOffset"}};


            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();

            var result = await client.PostAsync(
                $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/typeUpdate",
                testEntityV1.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var testEntityV2 = new Dictionary<string, object> {{"var", DateTime.Now}, {"EntityName", "DateTime"}};


            var ver1Json = await client.PatchAsync(
                $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/typeUpdate",
                testEntityV2.Serialize());
            Assert.Equal(HttpStatusCode.OK, ver1Json.StatusCode);

            var configFound =
                await client.GetAsync(
                    $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/typeUpdate/JsonRefresh");
            Assert.Equal(HttpStatusCode.OK, configFound.StatusCode);

            var ver1Data = configFound.Deserialize<dynamic>();
            Assert.Equal(ver1Data.var, testEntityV2["var"]);
            Assert.Equal(ver1Data.EntityName, testEntityV2["EntityName"]);

        }

        [Fact]
        public async Task It_should_update_null_field()
        {
            var testEntityV1 = new Dictionary<string, object> {{"nullProp", null}, {"EntityName", "checkNull"}};


            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();

            var result = await client.PostAsync(
                $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/nullField",
                testEntityV1.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var testEntityV2 = new Dictionary<string, object> {{"nullProp", "notnull"}, {"EntityName", "checkNull"}};


            var ver1Json = await client.PatchAsync(
                $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/nullField",
                testEntityV2.Serialize());
            Assert.Equal(HttpStatusCode.OK, ver1Json.StatusCode);

            var configFound =
                await client.GetAsync(
                    $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/nullField/json");
            Assert.Equal(HttpStatusCode.OK, configFound.StatusCode);

            var ver1Data = configFound.Deserialize<dynamic>();
            Assert.Equal(ver1Data.nullProp, testEntityV2["nullProp"]);
            Assert.Equal(ver1Data.EntityName, testEntityV2["EntityName"]);
        }

        [Fact]
        public async Task It_should_update_notnull_field()
        {
            var testEntityV1 = new TestClass() { Prop = "notNull" };


            using var server = TestServer(UserRole.Administrator);
            using var client = server.CreateHttpClient();

            var result = await client.PostAsync(
                $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/nullField",
                testEntityV1.Serialize());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var testEntityV2 = new TestClass() { Prop = null };

            var ver1Json = await client.PatchAsync(
                $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/nullField",
                testEntityV2.Serialize());
            Assert.Equal(HttpStatusCode.OK, ver1Json.StatusCode);

            var configFound =
                await client.GetAsync(
                    $"features/application/{_env2.Application.Code}/environment/{_env2.Code}/config/nullField/json");
            Assert.Equal(HttpStatusCode.OK, configFound.StatusCode);

            var ver1Data = configFound.Deserialize<TestClass>();
            Assert.Null(ver1Data.Prop);
            Assert.Equal(ver1Data.Prop, testEntityV2.Prop);
            Assert.Equal(ver1Data.EntityName, testEntityV2.EntityName);
        }

        class TestClass
        {
            public string Prop { get; set; }
            public string EntityName { get; set; } = "NullProperty";
        }
    }
}
