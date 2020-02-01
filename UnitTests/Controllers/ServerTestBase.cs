using Microsoft.AspNetCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ObjectConfig;
using ObjectConfig.Data;
using ObjectConfig.Features.Users;
using System;
using UnitTests.Mock;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class ServerTestBase : BaseTest
    {
        private static readonly Action<IServiceCollection> EmptyMethod = (s) => { };

        static ServerTestBase()
        {
            System.Environment.SetEnvironmentVariable("ASPNETCORE_Environment", "Development");
        }

        public ServerTestBase(ITestOutputHelper output) : base(output)
        {
        }

        protected virtual string[] Args => new string[] { };


        protected virtual Action<IServiceCollection> DefaultServices(Action<IServiceCollection> services)
        {
            return (s) =>
            {
                s.AddLogging((builder) => builder.AddXUnit(Log));
                s.AddObjectConfigContext(ConfigureDb);
                services(s);
            };
        }

        protected virtual void SeedData(ObjectConfigContext context, MockUserProvider userProvider)
        {

        }

        /// <summary>
        /// Configure virtual app server and seed data
        /// </summary>
        /// <param name="userRole">use provider role</param>
        /// <param name="services">func for mock configure</param>
        /// <returns></returns>
        protected virtual TestServer TestServer(User.Role userRole, Action<IServiceCollection> services = null)
        {
            var userProvider = new MockUserProvider(userRole);
            return TestServer(userProvider, services);
        }

        protected TestServer TestServer(MockUserProvider userProvider, Action<IServiceCollection> services = null)
        {
            Action<IServiceCollection> internalAction = (s) =>
            {
                services?.Invoke(s);
                s.AddSingleton<IUserProvider>(userProvider);
            };

            var testServer = TestServer(internalAction);
            testServer.CreateClient();

            var (instance, scope) = testServer.GetInstanceFromScope<ObjectConfigContext>();
            instance.Database.EnsureCreated();
            SeedData(instance, userProvider);
            instance.SaveChanges();
            instance.Dispose();
            scope.Dispose();
            return testServer;
        }

        protected virtual TestServer TestServer(Action<IServiceCollection> services = null)
        {
            var overideServices = services ?? EmptyMethod;
            var server = new TestServer(
                WebHost.CreateDefaultBuilder<Startup>(Args)
                .ConfigureTestServices(DefaultServices(overideServices)));

            return server;
        }
    }
}
