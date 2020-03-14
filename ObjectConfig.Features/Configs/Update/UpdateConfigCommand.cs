using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;

namespace ObjectConfig.Features.Configs.Update
{
    public class UpdateConfigCommand : ConfigArgumentCommand, IRequest<Config>
    {
        public UpdateConfigCommand(string applicationCode, string environmentCode, string configCode)
            : base(applicationCode, environmentCode, configCode)
        {
        }
    }
}
