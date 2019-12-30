using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ObjectConfig.Data.Configurations
{
    public class ConfigConfiguration : IEntityTypeConfiguration<Config>
    {
        public void Configure(EntityTypeBuilder<Config> builder)
        {
            builder.HasKey(p => p.ConfigId);
            builder.Property(p => p.ConfigId).UseHiLo();
           
            builder.Property(s => s.Code).IsRequired().HasMaxLength(128);

            builder.Property(s => s.VersionFrom).IsRequired().HasMaxLength(23);

            builder.Property(s => s.VersionTo).HasMaxLength(23);

            builder.Property(s => s.Description).HasMaxLength(512);

            builder.HasIndex(p => new { p.Code, p.VersionFrom, p.EnvironmentId }).IsUnique();
        }
    }
}
