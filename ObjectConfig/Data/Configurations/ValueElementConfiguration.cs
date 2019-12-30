using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ObjectConfig.Data.Configurations
{
    public class ValueElementConfiguration : IEntityTypeConfiguration<ValueElement>
    {
        public void Configure(EntityTypeBuilder<ValueElement> builder)
        {
            builder.HasKey(p => p.ValueElementId);
            builder.Property(p => p.ValueElementId).UseHiLo();

            builder.Property(p => p.Value).HasMaxLength(int.MaxValue);

            builder.Property(p => p.Comment).HasMaxLength(int.MaxValue);

            builder.Property(p => p.Comment).HasMaxLength(int.MaxValue);
        }
    }
}
