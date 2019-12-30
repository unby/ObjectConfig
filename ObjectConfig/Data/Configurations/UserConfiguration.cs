using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ObjectConfig.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(p => p.UserId);
            builder.Property(p => p.UserId).UseHiLo();

            builder.Property(s => s.ExternalId).HasMaxLength(256);
            builder.HasIndex(s => s.ExternalId).IsUnique();

            builder.Property(s => s.DisplayName).IsRequired().HasMaxLength(128);
            builder.Property(s => s.IsGlobalAdmin).IsRequired();

            builder.Property(s => s.Email).IsRequired().HasMaxLength(128);
            builder.HasIndex(s => s.Email).IsUnique();
        }
    }
}
