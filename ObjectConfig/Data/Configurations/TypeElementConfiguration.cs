using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace ObjectConfig.Data.Configurations
{
    public class TypeElementConfiguration : ConfigurationBase<TypeElement>
    {
        public TypeElementConfiguration(ModelBuilder modelBuilder, string dbType, int increment = 5, int startsAt = 100) : base(modelBuilder, dbType, increment, startsAt)
        {
        }

        protected override PropertyInfo KeyProperty => GetPKType(l => l.TypeElementId);

        protected override void ConfigureProperty(EntityTypeBuilder<TypeElement> builder)
        {
            builder.Property(s => s.Name).IsRequired().HasMaxLength(256);

            builder.Property(s => s.Description).HasMaxLength(512);
        }
    }
}
