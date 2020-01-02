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
            new ApplicationConfiguration(modelBuilder);
            new ConfigConfiguration(modelBuilder);
            new ConfigElementConfiguration(modelBuilder, 150);
            new EnvironmentConfiguration(modelBuilder);
            new TypeElementConfiguration(modelBuilder, 150);
            new UserConfiguration(modelBuilder);
            modelBuilder.ApplyConfiguration(new UsersApplicationsConfiguration());
            modelBuilder.ApplyConfiguration(new UsersEnvironmentsConfiguration());
            modelBuilder.ApplyConfiguration(new UsersTypesConfiguration());
            new ValueElementConfiguration(modelBuilder, 150);

            modelBuilder.Entity<User>().HasData(new User(Constants.AdminId, Guid.NewGuid().ToString(), "GlobalAdmin", "admin@global.net", true));
        }
        public DbSet<User> Users { get; set; }

        public DbSet<UsersApplications> UsersApplications { get; set; }

        public DbSet<UsersEnvironments> UsersEnvironments { get; set; }

        public DbSet<UsersTypes> UsersTypes { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<Environment> Environments { get; set; }

        public DbSet<Config> Configs { get; set; }

        public DbSet<ConfigElement> ConfigElements { get; set; }

        public DbSet<TypeElement> TypeElements { get; set; }

        public DbSet<ValueElement> ValueElements { get; set; }
    }
}
