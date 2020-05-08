using MediatR;
using ObjectConfig.Model;
using System.Threading;
using System.Threading.Tasks;
using ObjectConfig.Data;

namespace ObjectConfig.Features.Configs.JsonConverter
{
    public class JsonConverterHandler : IRequestHandler<JsonConverterCommand, string>
    {
        private readonly ConfigService _configService;
        private readonly CacheService _cacheService;

        public JsonConverterHandler(ConfigService configService, CacheService cacheService)
        {
            _configService = configService;
            _cacheService = cacheService;
        }

        public async Task<string> Handle(JsonConverterCommand request, CancellationToken cancellationToken)
        {
            var k = await _configService.GetConfig(request, cancellationToken);
            return await _cacheService.GetGonfig(k.ConfigId, cancellationToken);
        }
    }
}
