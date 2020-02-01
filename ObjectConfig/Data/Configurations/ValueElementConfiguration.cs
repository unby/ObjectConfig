using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace ObjectConfig.Data.Configurations
{
    public class ValueElementConfiguration : ConfigurationBase<ValueElement>
    {
        public ValueElementConfiguration(ModelBuilder modelBuilder, string dbType, int increment = 5, int startsAt = 100) : base(modelBuilder, dbType, increment, startsAt)
        {
        }

        protected override PropertyInfo KeyProperty => GetPKType(k => k.ValueElementId);

        protected override void ConfigureProperty(EntityTypeBuilder<ValueElement> builder)
        {
            builder.Property(p => p.Value).HasMaxLength(int.MaxValue);

            builder.Property(p => p.Comment).HasMaxLength(256);
        }
    }
}
