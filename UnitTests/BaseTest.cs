using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public class BaseTest
    {
        protected ObjectConfigContext GetObjectConfigContext()
        {
            DbContextOptions<ObjectConfigContext> options;
            var builder = new DbContextOptionsBuilder<ObjectConfigContext>();
            builder.UseInMemoryDatabase("nunit");
            options = builder.Options;
            ObjectConfigContext personDataContext = new ObjectConfigContext(options);
            personDataContext.Database.EnsureDeleted();
            personDataContext.Database.EnsureCreated();

            return personDataContext;
        }

        private readonly ITestOutputHelper Log;

        public BaseTest(ITestOutputHelper output)
        {
            this.Log = output;
        }

        [Fact]
        public void ChekUser()
        {
            using (var context = GetObjectConfigContext())
            {

                var admin = context.Admin();
                Assert.Equal(1, admin.UserId);
                Assert.Equal("GlobalAdmin", admin.DisplayName);
            }
        }



        [Fact]
        public void Test1()
        {
            using (var context = GetObjectConfigContext())
            {
                var admin = context.Admin();

                context.UsersApplications.Add(new UsersApplications() { User = admin, AccessRole = UsersApplications.Role.Administrtor, Application = new Application() { Code = "test", Name = "test title" } });

                //  context.UsersApplications.Append
                context.SaveChanges();
                Assert.True(context.UsersApplications.Any());
                Assert.True(context.Applications.Any());
                var app = context.Applications.First();
                Log.WriteLine(app.ApplicationId);
                Assert.NotNull(app);
            }
        }
    }
}
