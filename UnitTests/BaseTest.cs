using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ObjectConfig;
using ObjectConfig.Data;
using System;
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

        protected IServiceProvider GetDi(Func<IServiceCollection, IServiceCollection> func = null)
        {
            var sc = new ServiceCollection().AddLogging((builder) => builder.AddXUnit(Log)).ObjectConfigServices((a) => {
                if (intgration)
                    a.UseSqlServer(@"Data Source=localhost;Initial Catalog=ObjectConfig;Integrated Security=True;", opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
                else
                    a.UseInMemoryDatabase("xunit"); });
            if (func != null)
                sc = func(sc);
            return sc.BuildServiceProvider().CreateScope().ServiceProvider;
        }

        protected IServiceScope GetScope(Func<IServiceCollection, IServiceCollection> func = null)
        {
            return GetDi(func).CreateScope();
        }

        protected readonly ITestOutputHelper Log;
        private bool intgration = true;

        public BaseTest(ITestOutputHelper output)
        {
            this.Log = output;
        }
    }
}
/*,
""Array"": [
""123"",
""sto""
]
*/
