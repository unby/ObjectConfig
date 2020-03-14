using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Users;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Configs.Create
{
    public class CreateConfigHandler : IRequestHandler<CreateConfigCommand, Config>
    {
        private readonly SecurityService _securityService;
        private readonly ObjectConfigContext _configContext;

        public CreateConfigHandler(SecurityService securityService, ObjectConfigContext configContext)
        {
            _securityService = securityService;
            _configContext = configContext;
        }

        public async Task<Config> Handle(CreateConfigCommand request, CancellationToken cancellationToken)
        {
            var user = await _securityService.GetCurrentUser();



            return null;
        }
    }
}
