using MediatR;
using ObjectConfig.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Environments.FindByCode
{
    public class FindByCodeEnvironmentsHandler : IRequestHandler<FindByCodeEnvironmentCommand, UsersEnvironments>
    {
        private readonly EnvironmentService _environmentMaster;

        public FindByCodeEnvironmentsHandler(EnvironmentService environmentMaster)
        {
            _environmentMaster = environmentMaster;
        }

        public Task<UsersEnvironments> Handle(FindByCodeEnvironmentCommand request, CancellationToken cancellationToken)
        {
            return _environmentMaster.GetEnvironment(request, cancellationToken);
        }
    }
}
