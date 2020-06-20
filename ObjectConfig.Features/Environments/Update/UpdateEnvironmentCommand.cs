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
    public class UpdateEnvironmentCommand
        : EnvironmentArgumentCommand, IRequest<UsersEnvironments>
    {
        public UpdateEnvironmentCommand(string applicationCode, string environmentCode, UpdateEnvironmentDto updateApplication)
            : base(applicationCode, environmentCode)
        {
            if (updateApplication.Definition != null)
            {
                EnvironmentDefinition = new Definition(
                    updateApplication.Definition.Name,
                    updateApplication.Definition.Description);
            }

            if (updateApplication.Users != null && updateApplication.Users.Any())
            {
                Users = new Lazy<ReadOnlyCollection<User>>(
                    () => new ReadOnlyCollection<User>(updateApplication.Users.
                        Select(s => new User(s.UserId, s.Role, s.Operation)).ToList()));
            }

            if (EnvironmentDefinition == null && Users == null)
            {
                throw new RequestException($"Parameters is invalid");
            }
        }

        public Definition? EnvironmentDefinition { get; }

        public Lazy<ReadOnlyCollection<User>>? Users { get; }
    }

    public class User
           : IUserAcessLevel<EnvironmentRole>
    {
        public User(int userId, EnvironmentRole role, EntityOperation operation)
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
        public EnvironmentRole Role { get; }
        public EntityOperation Operation { get; }
    }
}
