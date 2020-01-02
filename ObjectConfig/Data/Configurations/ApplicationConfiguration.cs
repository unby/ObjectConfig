using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ObjectConfig.Data.Configurations
{
    public abstract class ConfigurationBase<Entity> : IEntityTypeConfiguration<Entity> where Entity : class
    {
        protected ConfigurationBase(ModelBuilder modelBuilder, int increment = 5)
        {
            modelBuilder.ApplyConfiguration(this);
            ConfigureHilo(modelBuilder, increment);
        }

        public void ConfigureHilo(ModelBuilder modelBuilder, int increment)
        {
            modelBuilder.HasSequence(PrimeryKeyType, SequenceName).StartsAt(10).IncrementsBy(increment);
        }

        protected string SequenceName => $"{typeof(Entity).Name}_HiloSeq";

        public void Configure(EntityTypeBuilder<Entity> builder)
        {
            ConfigureProperty(builder);
        }
        protected abstract Type PrimeryKeyType { get; }
        protected abstract void ConfigureProperty(EntityTypeBuilder<Entity> builder);

        protected Type GetPKType<TProp>(Expression<Func<Entity, TProp>> expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("'expression' should be a member expression");
            }

            var propertyInfo = (PropertyInfo)body.Member;

            return propertyInfo.PropertyType;
        }
    }

    public class ApplicationConfiguration : ConfigurationBase<Application>
    {
        private ModelBuilder modelBuilder;

        public ApplicationConfiguration(ModelBuilder modelBuilder, int increment = 5) : base(modelBuilder, increment)
        {
        }

        protected override Type PrimeryKeyType => GetPKType(k => k.ApplicationId);

        protected override void ConfigureProperty(EntityTypeBuilder<Application> builder)
        {
            builder.HasKey(p => p.ApplicationId);

            builder.Property(p => p.ApplicationId).UseHiLo(SequenceName);

            builder.Property(s => s.Code).IsRequired().HasMaxLength(64);
            builder.HasIndex(u => u.Code).IsUnique();

            builder.Property(s => s.Name).IsRequired().HasMaxLength(128);

            builder.Property(s => s.Description).HasMaxLength(512);
        }
    }
}
