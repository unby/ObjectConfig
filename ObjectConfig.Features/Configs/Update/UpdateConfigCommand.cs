using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;

namespace ObjectConfig.Features.Configs.Update
{
    public class UpdateConfigCommand : ConfigArgumentCommand, IRequest<Config>
    {
        public UpdateConfigCommand(string applicationCode, string environmentCode, string configCode, string? versionFrom, string? versionTo) : base(applicationCode, environmentCode, configCode, versionFrom, versionTo)
        {
        }

        public Definition? EnvironmentDefinition { get; }
    }
}
