using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Users;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ObjectConfig.Features.Environments.Update
{
    public class UpdateEnvironmentCommand : EnvironmentArgumentCommand, IRequest<UsersEnvironments>
    {
        public UpdateEnvironmentCommand(string applicationCode, string environmentCode, UpdateEnvironmentDto updateApplication)
            : base(applicationCode, environmentCode)
        {
            if (updateApplication.ApplicationDefinition != null)
            {
                EnvironmentDefinition = new Definition(
                    updateApplication.ApplicationDefinition.Name,
                    updateApplication.ApplicationDefinition.Description);
            }

            if (updateApplication.Users != null && updateApplication.Users.Any())
            {
                Users = new Lazy<ReadOnlyCollection<UpdateEnvironmentCommand.User>>(
                    () => new ReadOnlyCollection<User>(updateApplication.Users.
                        Select(s => new User(s.UserId, s.Role, s.Operation)).ToList()));
            }

            if (EnvironmentDefinition == null && Users == null)
            {
                throw new RequestException($"Parameters is invalid");
            }
        }

        public Definition? EnvironmentDefinition { get; }

        public Lazy<ReadOnlyCollection<UpdateEnvironmentCommand.User>>? Users { get; }

        public class User
            : IUserAcessLevel<UsersEnvironments.Role>
        {
            public User(int userId, UsersEnvironments.Role role, EntityOperation operation)
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
            public UsersEnvironments.Role Role { get; }
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
