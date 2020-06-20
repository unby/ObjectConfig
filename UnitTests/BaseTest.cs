using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ObjectConfig;
using ObjectConfig.Data;
using System;
using System.Data.Common;
using Xunit.Abstractions;

namespace UnitTests
{
    public class BaseTest
    {
        protected IServiceProvider GetDi(Func<IServiceCollection, IServiceCollection> func = null)
        {
            IServiceCollection sc = new ServiceCollection().AddLogging((builder) => builder.AddXUnit(Log)).AddObjectConfigContext(ConfigureDb(CreateConnection())).AddRepositories();
            if (func != null)
            {
                sc = func(sc);
            }

            ServiceProvider sp = sc.BuildServiceProvider();
            ObjectConfigContext instance = sp.CreateScope().ServiceProvider.GetService<ObjectConfigContext>();

            instance.Database.EnsureCreated();
            instance.SaveChanges();
            instance.Dispose();
            return sp;
        }

        protected static DbConnection CreateConnection()
        {
            SqliteConnection sqliteConnection = new SqliteConnection($"Data Source={Guid.NewGuid()};Mode=Memory;Cache=Shared");
            sqliteConnection.Open();
            return sqliteConnection;
        }

        protected virtual Action<DbContextOptionsBuilder> ConfigureDb(DbConnection connection)
        {
            return (DbContextOptionsBuilder dbContextOptionsBuilder) =>
            {
                if (_integration)
                {
                    Log.WriteLine("mssql");
                    dbContextOptionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=ObjectConfig;Integrated Security=True;", opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
                }
                else
                {
                    Log.WriteLine("UseSqlite");

                    dbContextOptionsBuilder.UseSqlite(connection);
                }
            };
        }

        protected IServiceScope GetScope(Func<IServiceCollection, IServiceCollection> func = null)
        {
            return GetDi(func).CreateScope();
        }

        protected readonly ITestOutputHelper Log;
        protected bool _integration = false;

        public BaseTest(ITestOutputHelper output)
        {
            this.Log = output;
        }
    }
}