using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;

namespace ObjectConfig.Features.Configs.FindByCode
{
    public class FindConfigCommand : ConfigArgumentCommand, IRequest<Config>
    {
        public FindConfigCommand(string applicationCode, string environmentCode, string configCode, string? versionFrom, string? versionTo) : base(applicationCode, environmentCode, configCode, versionFrom, versionTo)
        {
        }
    }
}
