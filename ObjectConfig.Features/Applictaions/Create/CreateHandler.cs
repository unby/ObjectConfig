using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Users;
using ObjectConfig.Model;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Applictaions.Create
{
    public class UpdateHandler : IRequestHandler<CreateCommand, UsersApplications>
    {
        private readonly SecurityService _securityService;
        private readonly ApplicationRepository _applicationRepository;

        public UpdateHandler(SecurityService securityService, ApplicationRepository applicationRepository)
        {
            _securityService = securityService;
            _applicationRepository = applicationRepository;
        }

        public async Task<UsersApplications> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            await _securityService.CheckAccess(User.Role.Administrator);

            Application application = new Application(request.Name, request.Code, request.Description);
            var userApp = new UsersApplications(await _securityService.GetCurrentUser(), application, UsersApplications.Role.Administrator);
            application.Users.Add(userApp);

            await _applicationRepository.Create(application);

            return userApp;
        }
    }
}
