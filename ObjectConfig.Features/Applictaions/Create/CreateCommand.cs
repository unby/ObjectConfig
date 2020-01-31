using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Exceptions;

namespace ObjectConfig.Features.Applictaions.Create
{
    public class CreateCommand : IRequest<UsersApplications>
    {
        public CreateCommand(string name, string code, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new RequestException($"Parameter '{nameof(name)}' isn't should empty");
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new RequestException($"Parameter '{nameof(code)}' isn't should empty");
            }

            Name = name;
            Code = code;
            Description = description;
        }

        public string Name { get; }
        public string Code { get; }
        public string? Description { get; }
    }
}
