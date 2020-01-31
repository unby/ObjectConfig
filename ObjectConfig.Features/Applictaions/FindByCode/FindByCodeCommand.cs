using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Exceptions;

namespace ObjectConfig.Features.Applictaions.FindByCode
{
    public class FindByCodeCommand : IRequest<UsersApplications>
    {
        public FindByCodeCommand(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new RequestException($"Parameter '{nameof(code)}' isn't should empty");
            }

            Code = code;
        }

        public string Code { get; }
    }
}
