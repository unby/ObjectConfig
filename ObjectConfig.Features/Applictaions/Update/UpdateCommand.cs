using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Users;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ObjectConfig.Features.Applictaions.Update
{
    public class UpdateCommand : ApplicationArgumentCommand, IRequest<UsersApplications>
    {
        public UpdateCommand(string code, UpdateApplicationDto updateApplication) : base(code)
        {
            if (updateApplication.ApplicationDefinition != null)
            {
                ApplicationDefinition = new Definition(
                    updateApplication.ApplicationDefinition.Name,
                    updateApplication.ApplicationDefinition.Description);
            }

            if (updateApplication.Users != null && updateApplication.Users.Any())
            {
                Users = new Lazy<ReadOnlyCollection<UpdateCommand.User>>(
                    () => new ReadOnlyCollection<User>(updateApplication.Users.
                        Select(s => new User(s.UserId, s.Role, s.Operation)).ToList()));
            }

            if (ApplicationDefinition == null && Users == null)
            {
                throw new RequestException($"Parameters is invalid");
            }
        }

        public Definition? ApplicationDefinition { get; }

        public Lazy<ReadOnlyCollection<UpdateCommand.User>>? Users { get; }

        public class User
            : IUserAcessLevel<ApplicationRole>
        {
            public User(int userId, ApplicationRole role, EntityOperation operation)
            {
                if (userId < 1)
                {
                    throw new RequestException($"Parameter '{nameof(userId)}' isn't correct  value");
                }

                UserId = userId;
                Role = role;
                Operation = operation;
            }

            public int UserId { get; }
            public ApplicationRole Role { get; }
            public EntityOperation Operation { get; }
        }

        public class Definition
        {
            public Definition(string name, string? description)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new RequestException($"Parameter '{nameof(name)}' isn't should empty");
                }

                Name = name;
                Description = description;
            }

            public string Name { get; }
            public string? Description { get; }
        }
    }
}
