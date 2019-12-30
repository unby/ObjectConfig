using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ObjectConfig.Data.Configurations
{
    public class UsersApplicationsConfiguration : IEntityTypeConfiguration<UsersApplications>
    {
        public void Configure(EntityTypeBuilder<UsersApplications> builder)
        {
            builder.HasKey(c => new { c.UserId, c.ApplicationId });
        }
    }
}
