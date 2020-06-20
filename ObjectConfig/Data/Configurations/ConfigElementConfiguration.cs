using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace ObjectConfig.Data.Configurations
{
    public class ConfigElementConfiguration
        : ConfigurationBase<ConfigElement>
    {
        public ConfigElementConfiguration(ModelBuilder modelBuilder, string dbType, int increment = 5, int startsAt = 100)
            : base(modelBuilder, dbType, increment, startsAt)
        {
        }

        protected override PropertyInfo KeyProperty => GetPKType(k => k.ConfigElementId);

        protected override void ConfigureProperty(EntityTypeBuilder<ConfigElement> builder)
        {
            builder.Property(p => p.Path).HasMaxLength(1024);
        }
    }
}
