using MediatR;
using ObjectConfig.Model;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Configs.JsonConverter
{
    public class JsonConverterHandler : IRequestHandler<JsonConverterCommand, string>
    {
        private readonly ConfigService _configService;
        private readonly ConfigElementRepository _configElementRepository;

        public JsonConverterHandler(ConfigService configService, ConfigElementRepository configElementRepository)
        {
            _configService = configService;
            _configElementRepository = configElementRepository;
        }

        public async Task<string> Handle(JsonConverterCommand request, CancellationToken cancellationToken)
        {
            var result = await _configService.GetConfig(request, cancellationToken);

            var element = (await _configElementRepository.GetConfigElement(result.ConfigId, cancellationToken)).root;

            return (await new JsonReducer().Parse(element)).ToString();
        }
    }
}
