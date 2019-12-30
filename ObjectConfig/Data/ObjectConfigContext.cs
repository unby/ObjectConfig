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
            modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
            modelBuilder.ApplyConfiguration(new ConfigConfiguration());
            modelBuilder.ApplyConfiguration(new ConfigElementConfiguration());
            modelBuilder.ApplyConfiguration(new EnvironmentConfiguration());
            modelBuilder.ApplyConfiguration(new TypeElementConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UsersApplicationsConfiguration());
            modelBuilder.ApplyConfiguration(new UsersEnvironmentsConfiguration());
            modelBuilder.ApplyConfiguration(new UsersTypesConfiguration());
            modelBuilder.ApplyConfiguration(new ValueElementConfiguration());

            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        UserId = Constants.AdminId,
                        DisplayName = "GlobalAdmin",
                        Email = "admin@global.net",
                        ExternalId = Guid.NewGuid().ToString(),
                        IsGlobalAdmin = true
                    }
                ); ;
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
