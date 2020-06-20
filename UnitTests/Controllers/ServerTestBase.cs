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
        private static readonly Action<IServiceCollection> _emptyMethod = (s) => { };

        static ServerTestBase()
        {
            System.Environment.SetEnvironmentVariable("ASPNETCORE_Environment", "Development");
        }

        public ServerTestBase(ITestOutputHelper output)
            : base(output)
        {
        }

        protected virtual string[] Args => Array.Empty<string>();

        protected virtual Action<IServiceCollection> DefaultServices(Action<IServiceCollection> services)
        {
            return (s) =>
            {
                s.AddLogging((builder) => builder.AddXUnit(Log));

                s.AddObjectConfigContext(ConfigureDb(CreateConnection()));
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
        /// <returns>Configured testserver</returns>
        protected virtual TestServer TestServer(UserRole userRole, Action<IServiceCollection> services = null)
        {
            MockUserProvider userProvider = new MockUserProvider(userRole);
            return TestServer(userProvider, services);
        }

        protected TestServer TestServer(MockUserProvider userProvider, Action<IServiceCollection> services = null)
        {
            void InternalAction(IServiceCollection s)
            {
                services?.Invoke(s);
                s.AddSingleton<IUserProvider>(userProvider);
            }

            TestServer testServer = TestServer(InternalAction);
            testServer.CreateClient();

            (ObjectConfigContext instance, IServiceScope scope) = testServer.GetInstanceFromScope<ObjectConfigContext>();
            instance.Database.EnsureCreated();
            SeedData(instance, userProvider);
            instance.SaveChanges();
            instance.Dispose();
            scope.Dispose();
            return testServer;
        }

        protected virtual TestServer TestServer(Action<IServiceCollection> services = null)
        {
            Action<IServiceCollection> overideServices = services ?? _emptyMethod;
            TestServer server = new TestServer(
                WebHost.CreateDefaultBuilder<Startup>(Args)
                .ConfigureTestServices(DefaultServices(overideServices)));

            return server;
        }
    }
}
