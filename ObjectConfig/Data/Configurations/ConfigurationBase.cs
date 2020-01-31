using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ObjectConfig.Data.Configurations
{
    public abstract class ConfigurationBase<Entity> : IEntityTypeConfiguration<Entity> where Entity : class
    {
        private readonly ModelBuilder _modelBuilder;
        private readonly string _dbType;
        private readonly int _increment;
        private readonly int _startsAt;
        private PropertyInfo _propertyInfo;

        protected ConfigurationBase(ModelBuilder modelBuilder, string dbType, int increment = 5, int startsAt = 100)
        {
            _modelBuilder = modelBuilder;
            _dbType = dbType;
            _increment = increment;
            _startsAt = startsAt;
            modelBuilder.ApplyConfiguration(this);
        }

        public void ConfigureHilo(ModelBuilder modelBuilder, int increment, int startsAt)
        {
            modelBuilder.HasSequence(KeyProperty.Name, SequenceName).StartsAt(startsAt).IncrementsBy(increment);
        }

        protected string SequenceName => $"{typeof(Entity).Name}_HiloSeq";

        public void Configure(EntityTypeBuilder<Entity> builder)
        {
            ConfigureProperty(builder);
            builder.HasKey(KeyProperty.Name);
            var keyProperty = builder.Property(KeyProperty.PropertyType, KeyProperty.Name); 

            if (_dbType.Contains("lite"))
            {
                keyProperty.ValueGeneratedOnAdd();
            }
            else
            {
                ConfigureHilo(_modelBuilder, _increment, _startsAt);
                keyProperty.UseHiLo(SequenceName);
            }
        }
        protected abstract PropertyInfo KeyProperty { get; }
        protected abstract void ConfigureProperty(EntityTypeBuilder<Entity> builder);

        protected PropertyInfo GetPKType<TProp>(Expression<Func<Entity, TProp>> expression)
        {
            if (_propertyInfo == null)
            {
                if (!(expression.Body is MemberExpression body))
                {
                    throw new ArgumentException("'expression' should be a member expression");
                }

                _propertyInfo = (PropertyInfo)body.Member;
            }
            return _propertyInfo;
        }
    }
}
