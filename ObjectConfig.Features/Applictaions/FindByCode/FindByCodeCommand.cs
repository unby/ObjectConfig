using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;

namespace ObjectConfig.Features.Applictaions.FindByCode
{
    public class FindByCodeCommand : ApplicationArgumentCommand, IRequest<UsersApplications>
    {
        public FindByCodeCommand(string code)
            : base(code)
        {
        }
    }
}
