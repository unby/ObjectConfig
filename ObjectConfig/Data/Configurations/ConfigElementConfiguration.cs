using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ObjectConfig.Data.Configurations
{
    public class ConfigElementConfiguration : ConfigurationBase<ConfigElement>
    {
        public ConfigElementConfiguration(ModelBuilder modelBuilder, int increment = 5) : base(modelBuilder, increment)
        {
        }

        protected override Type PrimeryKeyType => GetPKType(k=>k.ConfigElementId);

        protected override void ConfigureProperty(EntityTypeBuilder<ConfigElement> builder)
        {
            builder.HasKey(p => p.ConfigElementId);
            builder.Property(p => p.ConfigElementId).UseHiLo(SequenceName);
        }
    }
}
