using System.Collections.Generic;

namespace ObjectConfig.Data
{
    public class Environment : IUsers<UsersEnvironments, EnvironmentRole>
    {
        private Environment()
        {
        }

        public Environment(string name, string code, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException($"Constructor requires data for {nameof(Environment)}'s", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new System.ArgumentException($"Constructor requires data for {nameof(Environment)}'s", nameof(code));
            }

            Name = name;
            Code = code;
            Description = description;
        }

        public Environment(string name, string code, string? description, Application application)
            : this(name, code, description)
        {
            Application = application ?? throw new System.ArgumentNullException(nameof(application));
        }

        public int EnvironmentId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string? Description { get; set; }

        public int ApplicationId { get; set; }

        public Application Application { get; set; }

        public void Rename(string name)
        {
            Name = name;
        }

        public void NewDefinition(string? description)
        {
            Description = description;
        }

        public IList<Config> Configs { get; set; } = new List<Config>();

        public IList<UsersEnvironments> Users { get; set; } = new List<UsersEnvironments>();
    }
}
