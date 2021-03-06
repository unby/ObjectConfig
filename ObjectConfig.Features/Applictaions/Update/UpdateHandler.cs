﻿using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Applictaions.FindByCode;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Users;
using ObjectConfig.Model;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Applictaions.Update
{
    public class UpdateHandler : IRequestHandler<UpdateCommand, UsersApplications>
    {
        private readonly SecurityService _securityService;
        private readonly ApplicationRepository _applicationRepository;
        private readonly IMediator _mediator;
        private readonly ObjectConfigContext _configContext;

        public UpdateHandler(
            SecurityService securityService,
            ApplicationRepository applicationRepository,
            IMediator mediator,
            ObjectConfigContext configContext)
        {
            _securityService = securityService;
            _applicationRepository = applicationRepository;
            _mediator = mediator;
            _configContext = configContext;
        }

        public async Task<UsersApplications> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            Application application = await _applicationRepository.Find(request.ApplicationCode);

            request.ThrowNotFoundExceptionWhenValueIsNull(application);

            UsersApplications userApplication = await _securityService.CheckEntityAcces(application, ApplicationRole.Administrator);
            if (request.ApplicationDefinition != null)
            {
                application.Rename(request.ApplicationDefinition.Name);
                application.NewDefinition(request.ApplicationDefinition.Description);
            }
            else if (request.Users != null)
            {
                foreach (User changeUser in request.Users.Value.Where(w => w.UserId != userApplication?.UserId))
                {
                    UsersApplications foundUser = application.Users.FirstOrDefault(f => f.UserId == changeUser.UserId);
                    if (foundUser != null)
                    {
                        switch (changeUser.Operation)
                        {
                            case EntityOperation.Create:
                                _configContext.UsersApplications.Add(
                                    new UsersApplications(changeUser.UserId, application.ApplicationId, changeUser.Role));
                                break;
                            case EntityOperation.Update:
                                foundUser.AccessRole = changeUser.Role;
                                _configContext.UsersApplications.Update(foundUser);
                                break;
                            case EntityOperation.Delete:
                                _configContext.UsersApplications.Remove(foundUser);
                                break;
                        }
                    }
                }
            }

            await _configContext.SaveChangesAsync();

            return await _mediator.Send(new FindByCodeCommand(application.Code), cancellationToken);
        }
    }
}
