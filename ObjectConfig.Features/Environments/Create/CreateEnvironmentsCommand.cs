using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Common;

namespace ObjectConfig.Features.Environments.Create
{
    public class CreateEnvironmentsCommand : ApplicationArgumentCommand, IRequest<UsersEnvironments>
    {
        public CreateEnvironmentsCommand(string applicationCode, string name, string environmentCode, string? description)
                  : base(applicationCode)
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
            EnvironmentCode = environmentCode;
            Description = description;
        }

        public string Name { get; }
        public string EnvironmentCode { get; }
        public string? Description { get; }
    }
}
