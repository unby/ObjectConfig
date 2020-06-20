using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace ObjectConfig.Data.Configurations
{
    public class ConfigCacheConfiguration : ConfigurationBase<ConfigCache>
    {
        public ConfigCacheConfiguration(ModelBuilder modelBuilder, string dbType, int increment = 5, int startsAt = 100)
            : base(modelBuilder, dbType, increment, startsAt)
        {
        }

        protected override PropertyInfo KeyProperty => GetPKType(f => f.ConfigCacheId);

        protected override void ConfigureProperty(EntityTypeBuilder<ConfigCache> builder)
        {
            builder.HasKey(c => new { c.ConfigId });
            builder.Property(s => s.ConfigValue).IsRequired().HasMaxLength(int.MaxValue);
        }
    }
}
