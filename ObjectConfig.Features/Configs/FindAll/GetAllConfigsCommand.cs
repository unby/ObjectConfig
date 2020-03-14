using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using System.Collections.Generic;

namespace ObjectConfig.Features.Configs.FindAll
{
    public class GetAllConfigsCommand : EnvironmentArgumentCommand, IRequest<List<Config>>
    {
        public GetAllConfigsCommand(string applicationCode, string environmentCode) : base(applicationCode, environmentCode)
        {
        }
    }
}
