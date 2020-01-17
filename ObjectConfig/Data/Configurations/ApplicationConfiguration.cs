using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ObjectConfig.Data.Configurations
{

    public class ApplicationConfiguration : ConfigurationBase<Application>
    {
        public ApplicationConfiguration(ModelBuilder modelBuilder, int increment = 5) : base(modelBuilder, increment)
        {
        }

        protected override Type PrimeryKeyType => GetPKType(k => k.ApplicationId);

        protected override void ConfigureProperty(EntityTypeBuilder<Application> builder)
        {
            builder.HasKey(p => p.ApplicationId);

            builder.Property(p => p.ApplicationId).UseHiLo(SequenceName);

            builder.Property(s => s.Code).IsRequired().HasMaxLength(64);

            builder.HasIndex(u => u.Code).IsUnique();

            builder.Property(s => s.Name).IsRequired().HasMaxLength(128);

            builder.Property(s => s.Description).HasMaxLength(512);
        }
    }
}
