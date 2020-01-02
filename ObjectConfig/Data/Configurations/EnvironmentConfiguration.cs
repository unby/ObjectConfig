using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ObjectConfig.Data.Configurations
{
    public class EnvironmentConfiguration : ConfigurationBase<Environment>
    {
        public EnvironmentConfiguration(ModelBuilder modelBuilder, int increment = 5) : base(modelBuilder, increment)
        {
        }

        protected override Type PrimeryKeyType => GetPKType(k=>k.EnvironmentId);

        protected override void ConfigureProperty(EntityTypeBuilder<Environment> builder)
        {
            builder.HasKey(p => p.EnvironmentId);
            builder.Property(p => p.EnvironmentId).UseHiLo(SequenceName);

            builder.Property(s => s.Code).IsRequired().HasMaxLength(64);
            builder.HasIndex(p => new { p.Code, p.ApplicationId }).IsUnique();

            builder.Property(s => s.Name).IsRequired().HasMaxLength(128);

            builder.Property(s => s.Description).HasMaxLength(512);
        }
    }
}
