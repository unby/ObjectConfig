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
            return await _configService.GetConfigValue(
                () => _configService.GetConfig(request, cancellationToken),
                cancellationToken);
        }
    }
}
