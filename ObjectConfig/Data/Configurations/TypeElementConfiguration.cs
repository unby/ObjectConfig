using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ObjectConfig.Data.Configurations
{
    public class TypeElementConfiguration : ConfigurationBase<TypeElement>
    {
        public TypeElementConfiguration(ModelBuilder modelBuilder, int increment = 5) : base(modelBuilder, increment)
        {
        }

        protected override Type PrimeryKeyType => GetPKType(l => l.TypeElementId);

        protected override void ConfigureProperty(EntityTypeBuilder<TypeElement> builder)
        {
            builder.HasKey(p => p.TypeElementId);
            builder.Property(p => p.TypeElementId).UseHiLo(SequenceName);

            builder.Property(s => s.Name).IsRequired().HasMaxLength(256);

            builder.Property(s => s.Description).HasMaxLength(512);
        }
    }
}
