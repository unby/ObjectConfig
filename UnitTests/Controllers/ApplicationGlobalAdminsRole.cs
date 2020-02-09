﻿using ObjectConfig.Data;
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
    public class ApplicationGlobalAdminsRole : ServerTestBase
    {
        public ApplicationGlobalAdminsRole(ITestOutputHelper output) : base(output)
        {
        }

        protected override void SeedData(ObjectConfigContext context, MockUserProvider userProvider)
        {
            var app1 = DataSeed.Application1;
            var viewer = DataSeed.UserViewer1;
            context.UsersApplications.Add(new UsersApplications(viewer, app1, UsersApplications.Role.Viewer));
        }

        [Fact]
        public async Task It_should_create()
        {
            var testApp = new CreateApplicationDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString() };
            using var server = TestServer(User.Role.GlobalAdministrator);
            using var client = server.CreateHttpClient();
            var result = await client.PostAsync("feature/application", testApp.Serialize());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task It_should_FotFound()
        {
            var testApp = new CreateApplicationDto() { Code = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString() };
            using var server = TestServer(User.Role.GlobalAdministrator);
            using var client = server.CreateHttpClient();
            var result = await client.GetAsync("feature/application/notcode");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task It_should_update()
        {
            var testApp = DataSeed.Application1;
            using var server = TestServer(User.Role.GlobalAdministrator);
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
            Log.WriteLine(result.Content.ReadAsStringAsync().Result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var updateDto = result.Deserialize<ApplicationDTO>();

            Assert.Equal(updtestApp.ApplicationDefinition.Description, updateDto.Description);
            Assert.Equal(updtestApp.ApplicationDefinition.Name, updateDto.Name);
        }
    }
}