using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ObjectConfig.Data;
using System;

namespace ObjectConfig.Migrator
{
    public class ObjectConfigContextFactory : IDesignTimeDbContextFactory<ObjectConfigContext>
    {
        public ObjectConfigContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ObjectConfigContext>();
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=ObjectConfig;Integrated Security=True;", opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new ObjectConfigContext(optionsBuilder.Options);
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                new ObjectConfigContextFactory().CreateDbContext(args).Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
