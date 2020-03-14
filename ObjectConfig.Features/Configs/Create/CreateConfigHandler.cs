using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Users;
using System.Threading;
using System.Threading.Tasks;
using ObjectConfig.Features.Environments;

namespace ObjectConfig.Features.Configs.Create
{
    public class CreateConfigHandler : IRequestHandler<CreateConfigCommand, Config>
    {
        private readonly SecurityService _securityService;
        private readonly EnvironmentService _environmentService;
        private readonly ObjectConfigContext _configContext;

        public CreateConfigHandler(EnvironmentService environmentService, ObjectConfigContext configContext)
        {
            _environmentService = environmentService;
            _configContext = configContext;
        }

        public async Task<Config> Handle(CreateConfigCommand request, CancellationToken cancellationToken)
        { 
            var environment = await _environmentService.GetEnvironment(request, EnvironmentRole.Editor, cancellationToken);

            var config = new Config(request.ConfigCode, environment);

            var reader = new ObjectConfigReader(config);
            
            ConfigElement configElemnt = await reader.Parse(request.Data);

            _configContext.ConfigElements.Add(configElemnt);
            await _configContext.SaveChangesAsync();

            return config;
        }
    }
}
