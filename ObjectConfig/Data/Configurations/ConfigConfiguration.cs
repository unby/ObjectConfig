using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ObjectConfig.Data.Configurations
{
    public class ConfigConfiguration : ConfigurationBase<Config>
    {
        public ConfigConfiguration(ModelBuilder modelBuilder, int increment = 5) : base(modelBuilder, increment)
        {
        }

        protected override Type PrimeryKeyType => GetPKType(f => f.ConfigId);

        protected override void ConfigureProperty(EntityTypeBuilder<Config> builder)
        {
            builder.HasKey(p => p.ConfigId);
            builder.Property(p => p.ConfigId).UseHiLo(SequenceName);

            builder.Property(s => s.Code).IsRequired().HasMaxLength(128);

            builder.Property(s => s.VersionFrom).IsRequired();

            builder.Property(s => s.VersionTo);

            builder.Property(s => s.Description).HasMaxLength(512);

            builder.HasIndex(p => new { p.Code, p.VersionFrom, p.EnvironmentId }).IsUnique();

            builder.Ignore(s => s.RootConfigElement);
        }
    }
}
