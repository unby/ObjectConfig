using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ObjectConfig.Data.Configurations
{
    public class TypeElementConfiguration : IEntityTypeConfiguration<TypeElement>
    {
        public void Configure(EntityTypeBuilder<TypeElement> builder)
        {
            builder.HasKey(p => p.TypeElementId);
            builder.Property(p => p.TypeElementId).UseHiLo();

            builder.Property(s => s.Name).IsRequired().HasMaxLength(256);

            builder.Property(s => s.Description).HasMaxLength(512);
        }
    }
}
