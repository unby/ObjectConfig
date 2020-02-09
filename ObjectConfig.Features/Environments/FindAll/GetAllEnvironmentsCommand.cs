using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using System.Collections.Generic;

namespace ObjectConfig.Features.Environments.FindAll
{
    public class GetAllEnvironmentsCommand : ApplicationArgumentCommand, IRequest<List<UsersEnvironments>>
    {
        public GetAllEnvironmentsCommand(string applicationCode)
                  : base(applicationCode)
        {
        }
    }
}
