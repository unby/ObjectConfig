#nullable disable
using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data.Configurations;
using System;

namespace ObjectConfig.Data
{
    public class ObjectConfigContext : DbContext
    {
        public ObjectConfigContext(DbContextOptions<ObjectConfigContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new ApplicationConfiguration(modelBuilder, Database.ProviderName);
            new ConfigConfiguration(modelBuilder, Database.ProviderName);
            new ConfigElementConfiguration(modelBuilder, Database.ProviderName, 50);
            new EnvironmentConfiguration(modelBuilder, Database.ProviderName);
            new TypeElementConfiguration(modelBuilder, Database.ProviderName, 50);
            new UserConfiguration(modelBuilder, Database.ProviderName);
            new ConfigCacheConfiguration(modelBuilder, Database.ProviderName);
            modelBuilder.ApplyConfiguration(new UsersApplicationsConfiguration());
            modelBuilder.ApplyConfiguration(new UsersEnvironmentsConfiguration());
            modelBuilder.ApplyConfiguration(new UsersTypesConfiguration());
            new ValueElementConfiguration(modelBuilder, Database.ProviderName, 50);

            modelBuilder.Entity<User>().HasData(new User(Constants.AdminId, Guid.NewGuid().ToString(), "GlobalAdmin", "admin@global.net", UserRole.GlobalAdministrator));
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UsersApplications> UsersApplications { get; set; }

        public DbSet<UsersEnvironments> UsersEnvironments { get; set; }

        public DbSet<UsersTypes> UsersTypes { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<Environment> Environments { get; set; }

        public DbSet<Config> Configs { get; set; }

        public DbSet<ConfigElement> ConfigElements { get; set; }

        public DbSet<ConfigCache> ConfigCache { get; set; }

        public DbSet<TypeElement> TypeElements { get; set; }

        public DbSet<ValueElement> ValueElements { get; set; }
    }
}
