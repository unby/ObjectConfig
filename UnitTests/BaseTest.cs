using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ObjectConfig;
using ObjectConfig.Data;
using System;
using Xunit.Abstractions;

namespace UnitTests
{

    public class BaseTest : IDisposable
    {
        protected IServiceProvider GetDi(Func<IServiceCollection, IServiceCollection> func = null)
        {
            var sc = new ServiceCollection().AddLogging((builder) => builder.AddXUnit(Log)).AddObjectConfigContext(ConfigureDb).AddRepositories();
            if (func != null)
            {
                sc = func(sc);
            }

            var sp = sc.BuildServiceProvider();
            var instance = sp.CreateScope().ServiceProvider.GetService<ObjectConfigContext>();

            instance.Database.EnsureCreated();
            instance.SaveChanges();
            instance.Dispose();
            return sp;
        }

        public void Dispose()
        {
            this.DisposeInternal(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void ConfigureDb(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (_integration)
            {
                Log.WriteLine("mssql");
                dbContextOptionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=ObjectConfig;Integrated Security=True;", opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            }
            else
            {
                Log.WriteLine("UseSqlite");
                dbContextOptionsBuilder.UseSqlite(SqliteConnection);
            }
        }

        protected SqliteConnection SqliteConnection { get; set; }

        protected virtual void Dispose(bool disposing)
        {
        }

        private void DisposeInternal(bool disposing)
        {
            if (disposing)
            {
                SqliteConnection.Dispose();
                Dispose(disposing);
            }
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
            SqliteConnection = new SqliteConnection($"Data Source={GetType().Name};Mode=Memory;Cache=Shared"); //"DataSource=:memory:");
            SqliteConnection.Open();
        }
    }
}