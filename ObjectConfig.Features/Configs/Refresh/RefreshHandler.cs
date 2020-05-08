using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ObjectConfig.Data;

namespace ObjectConfig.Features.Configs.Refresh
{
    public class RefreshHandler: IRequestHandler<RefreshCommand, string>
    {
        private readonly ConfigService _configService;
        private readonly CacheService _cacheService;
        private readonly ObjectConfigContext _configContext;

        public RefreshHandler(ConfigService configService, CacheService cacheService, ObjectConfigContext configContext)
        {
            _configService = configService;
            _cacheService = cacheService;
            _configContext = configContext;
        }

        public async Task<string> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var configSource = await _configService.GetConfigElement(
                () => _configService.GetConfig(request, EnvironmentRole.Editor,cancellationToken),
                cancellationToken);
            var result= (await new JsonReducer().Parse(configSource.root)).ToString();
            await _cacheService.UpdateJsonConfig(configSource.config.ConfigId, result, cancellationToken);
            return result;
        }
    }
}
