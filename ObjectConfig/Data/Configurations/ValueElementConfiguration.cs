using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ObjectConfig.Data.Configurations
{
    public class ValueElementConfiguration : ConfigurationBase<ValueElement>
    {
        public ValueElementConfiguration(ModelBuilder modelBuilder, int increment = 5) : base(modelBuilder, increment)
        {
        }

        protected override Type PrimeryKeyType => GetPKType(k => k.ValueElementId);

        protected override void ConfigureProperty(EntityTypeBuilder<ValueElement> builder)
        {
            builder.HasKey(p => p.ValueElementId);
            builder.Property(p => p.ValueElementId).UseHiLo(SequenceName);

            builder.Property(p => p.Value).HasMaxLength(int.MaxValue);

            builder.Property(p => p.Comment).HasMaxLength(int.MaxValue);

            builder.Property(p => p.Comment).HasMaxLength(int.MaxValue);
        }
    }
}
