using ObjectConfig.Exceptions;

namespace ObjectConfig.Features.Common
{
    public class Definition
    {
        public Definition(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new RequestException($"Parameter '{nameof(name)}' isn't should empty");
            }

            Name = name;
            Description = description;
        }

        public string? Description { get; }

        public string Name { get; }
    }
}
