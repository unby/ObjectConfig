using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace ObjectConfig.Data.Configurations
{
    public class UserConfiguration : ConfigurationBase<User>
    {
        public UserConfiguration(ModelBuilder modelBuilder, string dbType, int increment = 5, int startsAt = 100) : base(modelBuilder, dbType, increment, startsAt)
        {
        }

        protected override PropertyInfo KeyProperty => GetPKType(k => k.UserId);

        protected override void ConfigureProperty(EntityTypeBuilder<User> builder)
        {
            // builder.HasKey(p => p.UserId);
            // builder.Property(p => p.UserId).UseHiLo(SequenceName);

            builder.Property(s => s.ExternalId).HasMaxLength(256);
            builder.HasIndex(s => s.ExternalId).IsUnique();

            builder.Property(s => s.DisplayName).IsRequired().HasMaxLength(128);

            builder.Property(s => s.Email).IsRequired().HasMaxLength(128);
            builder.HasIndex(s => s.Email).IsUnique();
        }
    }
}
