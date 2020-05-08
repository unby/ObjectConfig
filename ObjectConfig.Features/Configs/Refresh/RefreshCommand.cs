using MediatR;
using ObjectConfig.Features.Common;

namespace ObjectConfig.Features.Configs.Refresh
{
    public class RefreshCommand : ConfigArgumentCommand, IRequest<string>
    {
        public RefreshCommand(string applicationCode, string environmentCode, string configCode, string? versionFrom)
            : base(applicationCode, environmentCode, configCode, versionFrom)
        {
        }
    }
}
