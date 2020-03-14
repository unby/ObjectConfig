using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Common;

namespace ObjectConfig.Features.Configs.Create
{
    public class CreateConfigCommand : EnvironmentArgumentCommand, IRequest<Config>
    {
        public CreateConfigCommand(string applicationCode, string environmentCode, string configCode, string name, string? description)
                  : base(applicationCode, environmentCode)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new RequestException($"Parameter '{nameof(name)}' isn't should empty");
            }

            if (string.IsNullOrWhiteSpace(environmentCode))
            {
                throw new RequestException($"Parameter '{nameof(environmentCode)}' isn't should empty");
            }

            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string? Description { get; }
    }
}
