using Microsoft.EntityFrameworkCore;
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
            var usersApplicationsModel = modelBuilder.Entity<UsersApplications>();
            usersApplicationsModel.HasKey(c => new { c.UserId, c.ApplicationId });
         //   usersApplicationsModel.HasOne(o => o.User).WithMany(m => m.Applications);
         //   usersApplicationsModel.HasOne(o => o.Application).WithMany(m => m.Users);

            var usersEnvironmentsModel = modelBuilder.Entity<UsersEnvironments>();
            usersEnvironmentsModel.HasKey(c => new { c.UserId, c.EnvironmentId });
         //  usersEnvironmentsModel.HasOne(o => o.User).WithMany(m => m.Environments);
         //  usersEnvironmentsModel.HasOne(o => o.Environment).WithMany(m => m.Users);

            modelBuilder.Entity<UsersTypes>()
                .HasKey(c => new { c.UserId, c.ValueTypeId });


            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        UserId = 1,
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

        public DbSet<ValueConfig> ValueConfigs { get; set; }

        public DbSet<ValueType> ValueTypes { get; set; }

        public DbSet<ValueObject> ValueObjects { get; set; }
    }
}
