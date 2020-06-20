using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace ObjectConfig.Data.Configurations
{
    public class ApplicationConfiguration : ConfigurationBase<Application>
    {
        public ApplicationConfiguration(ModelBuilder modelBuilder, string dbType, int increment = 5, int startsAt = 100)
            : base(modelBuilder, dbType, increment, startsAt)
        {
        }

        protected override PropertyInfo KeyProperty => GetPKType(k => k.ApplicationId);

        protected override void ConfigureProperty(EntityTypeBuilder<Application> builder)
        {
            builder.Property(s => s.Code).IsRequired().HasMaxLength(64);

            builder.HasIndex(u => u.Code).IsUnique();

            builder.Property(s => s.Name).IsRequired().HasMaxLength(128);

            builder.Property(s => s.Description).HasMaxLength(512);
        }
    }
}
