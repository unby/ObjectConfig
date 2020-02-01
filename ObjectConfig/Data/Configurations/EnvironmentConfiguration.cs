using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace ObjectConfig.Data.Configurations
{
    public class EnvironmentConfiguration : ConfigurationBase<Environment>
    {
        public EnvironmentConfiguration(ModelBuilder modelBuilder, string dbType, int increment = 5, int startsAt = 100) : base(modelBuilder, dbType, increment, startsAt)
        {
        }

        protected override PropertyInfo KeyProperty => GetPKType(k => k.EnvironmentId);

        protected override void ConfigureProperty(EntityTypeBuilder<Environment> builder)
        {
            builder.Property(s => s.Code).IsRequired().HasMaxLength(64);
            builder.HasIndex(p => new { p.Code, p.ApplicationId }).IsUnique();

            builder.Property(s => s.Name).IsRequired().HasMaxLength(128);

            builder.Property(s => s.Description).HasMaxLength(512);
        }
    }
}
