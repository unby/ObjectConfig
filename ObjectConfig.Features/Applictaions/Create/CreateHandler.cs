using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Users;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Applictaions.Create
{
    public class CreateHandler : IRequestHandler<CreateCommand, UsersApplications>
    {
        private readonly SecurityService _securityService;
        private readonly ObjectConfigContext _configContext;

        public CreateHandler(SecurityService securityService, ObjectConfigContext configContext)
        {
            _securityService = securityService;
            _configContext = configContext;
        }

        public async Task<UsersApplications> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            await _securityService.CheckAccess(UserRole.Administrator);

            Application application = new Application(request.Name, request.Code, request.Description);
            UsersApplications userApp = new UsersApplications(await _securityService.GetCurrentUser(), application, ApplicationRole.Administrator);
            application.Users.Add(userApp);

            _configContext.Applications.Add(application);
            await _configContext.SaveChangesAsync(cancellationToken);

            return userApp;
        }
    }
}
