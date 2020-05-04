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

        public JsonConverterHandler(ConfigService configService)
        {
            _configService = configService;
        }

        public async Task<string> Handle(JsonConverterCommand request, CancellationToken cancellationToken)
        {
#if debug
            return await _configService.GetConfigValue(
                () => _configService.GetConfig(request, cancellationToken),
                cancellationToken);
#endif
            var k= await _configService.GetConfigElement(
                () => _configService.GetConfig(request, EnvironmentRole.Editor,cancellationToken),
                cancellationToken);
           return (await new JsonReducer().Parse(k.root)).ToString();
        }
    }
}
