using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ObjectConfig.Data.Configurations
{
    public class ConfigElementConfiguration : IEntityTypeConfiguration<ConfigElement>
    {
        public void Configure(EntityTypeBuilder<ConfigElement> builder)
        {
            builder.HasKey(p => p.ConfigElementId);
            builder.Property(p => p.ConfigElementId).UseHiLo();
        }
    }
}
