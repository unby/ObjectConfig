using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ObjectConfig.Data.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasKey(p => p.ApplicationId);
            builder.Property(p => p.ApplicationId).UseHiLo();

            builder.Property(s => s.Code).IsRequired().HasMaxLength(64);
            builder.HasIndex(u => u.Code).IsUnique();

            builder.Property(s => s.Name).IsRequired().HasMaxLength(128);

            builder.Property(s => s.Description).HasMaxLength(512);
        }
    }
}
