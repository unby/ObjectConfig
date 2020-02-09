using ObjectConfig.Data;

namespace ObjectConfig.Features.Environments
{
    public class EnvironmentDto
    {
#nullable disable
        private EnvironmentDto()
        {
        }
#nullable enable
        public EnvironmentDto(string code, string name, string? description, UsersEnvironments.Role role)
        {
            Code = code;
            Name = name;
            Description = description;
            Role = role;
        }

        public EnvironmentDto(UsersEnvironments usersEnvironments)
        {
            if (usersEnvironments is null)
            {
                throw new System.ArgumentNullException(nameof(usersEnvironments));
            }

            Code = usersEnvironments.Environment.Code;
            Name = usersEnvironments.Environment.Name;
            Description = usersEnvironments.Environment.Description;
            Role = usersEnvironments.AccessRole;
        }

        public string Code { get; private set; }

        public string Name { get; protected set; }

        public string? Description { get; protected set; }

        public UsersEnvironments.Role Role { get; protected set; }
    }
}
