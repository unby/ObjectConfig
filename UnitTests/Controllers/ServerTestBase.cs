using Microsoft.AspNetCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
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
                s.ObjectConfigServices((a) =>
                {
                    if (_integration)
                    {
                        a.UseSqlServer(@"Data Source=localhost;Initial Catalog=ObjectConfig;Integrated Security=True;", opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
                    }
                    else
                    {
                        a.UseInMemoryDatabase("xunit");
                    }
                });

                services(s);
            };
        }
        protected virtual TestServer TestServer(User.Role userRole, Action<IServiceCollection> services = null)
        {
            
            return TestServer(new MockUserProvider(userRole), services);
        }
        protected virtual TestServer TestServer(IUserProvider userProvider, Action<IServiceCollection> services = null)
        {
            Action<IServiceCollection> intenalAction = (s) =>
            {
                services?.Invoke(s);
                s.AddSingleton(userProvider);
            };
            return TestServer(intenalAction);
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
