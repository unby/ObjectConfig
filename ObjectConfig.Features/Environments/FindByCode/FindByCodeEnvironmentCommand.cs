using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;

namespace ObjectConfig.Features.Environments.FindByCode
{
    public class FindByCodeEnvironmentCommand : EnvironmentArgumentCommand, IRequest<UsersEnvironments>
    {
        public FindByCodeEnvironmentCommand(string applicationCode, string environmentCode)
                  : base(applicationCode, environmentCode)
        {
        }
    }
}
