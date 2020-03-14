using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Common;

namespace ObjectConfig.Features.Configs.Create
{
    public class CreateConfigCommand : ConfigArgumentCommand, IRequest<Config>
    {
        public CreateConfigCommand(string applicationCode, string environmentCode, string configCode, string data, string? versionFrom)
                  : base(applicationCode, environmentCode, configCode, versionFrom)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new RequestException($"Parameter '{nameof(data)}' isn't should empty");
            }

            Data = data;
        }

        public string Data { get; }
    }
}
