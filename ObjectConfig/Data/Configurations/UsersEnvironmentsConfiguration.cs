using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ObjectConfig.Data.Configurations
{
    public class UsersEnvironmentsConfiguration : IEntityTypeConfiguration<UsersEnvironments>
    {
        public void Configure(EntityTypeBuilder<UsersEnvironments> builder)
        {
            builder.HasKey(c => new { c.UserId, c.EnvironmentId });
        }
    }
}
