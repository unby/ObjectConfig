using MediatR;
using ObjectConfig.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Configs.FindConfig
{
    public class FindConfigHandler : IRequestHandler<FindConfigCommand, Config>
    {
        private readonly ConfigService _configService;

        public FindConfigHandler(ConfigService configService)
        {
            _configService = configService;
        }

        public async Task<Config> Handle(FindConfigCommand request, CancellationToken cancellationToken)
        {
            Config k = await _configService.GetConfig(request, cancellationToken);
            return k;
        }
    }
}
