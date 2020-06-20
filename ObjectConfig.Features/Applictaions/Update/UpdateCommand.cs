using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Common;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ObjectConfig.Features.Applictaions.Update
{
    public class UpdateCommand : ApplicationArgumentCommand, IRequest<UsersApplications>
    {
        public UpdateCommand(string code, UpdateApplicationDto updateApplication)
            : base(code)
        {
            if (updateApplication.ApplicationDefinition != null)
            {
                ApplicationDefinition = new Definition(
                    updateApplication.ApplicationDefinition.Name,
                    updateApplication.ApplicationDefinition.Description);
            }

            if (updateApplication.Users != null && updateApplication.Users.Any())
            {
                Users = new Lazy<ReadOnlyCollection<User>>(
                    () => new ReadOnlyCollection<User>(updateApplication.Users.
                        Select(s => new User(s.UserId, s.Role, s.Operation)).ToList()));
            }

            if (ApplicationDefinition == null && Users == null)
            {
                throw new RequestException($"Parameters is invalid");
            }
        }

        public Definition? ApplicationDefinition { get; }

        public Lazy<ReadOnlyCollection<User>>? Users { get; }
    }
}
