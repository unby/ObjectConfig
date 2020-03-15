using MediatR;
using ObjectConfig.Features.Common;

namespace ObjectConfig.Features.Configs.JsonConverter
{
    public class JsonConverterCommand : ConfigArgumentCommand, IRequest<string>
    {
        public JsonConverterCommand(string applicationCode, string environmentCode, string configCode, string? versionFrom)
                : base(applicationCode, environmentCode, configCode, versionFrom)
        {
        }
    }
}
