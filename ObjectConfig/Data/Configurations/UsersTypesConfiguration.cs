using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ObjectConfig.Data.Configurations
{
    public class UsersTypesConfiguration : IEntityTypeConfiguration<UsersTypes>
    {
        public void Configure(EntityTypeBuilder<UsersTypes> builder)
        {
            builder.HasKey(c => new { c.UserId, c.TypeElementId });
        }
    }
}
