using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;

namespace ObjectConfig.Features.Environments.FindByCode
{
    public class FindByCodeEnvironmentCommand : ApplicationArgumentCommand, IRequest<UsersEnvironments>
    {
        public FindByCodeEnvironmentCommand(string applicationCode, string environmentCode)
                  : base(applicationCode)
        {
            EnvironmentCode = environmentCode;
        }

        public string EnvironmentCode { get; }
    }
}
