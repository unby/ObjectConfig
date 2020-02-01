using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Users;
using ObjectConfig.Model;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Applictaions.FindByCode
{
    public class FindByCodeHandler : IRequestHandler<FindByCodeCommand, UsersApplications>
    {
        private readonly SecurityService _securityService;
        private readonly ApplicationRepository _applicationRepository;

        public FindByCodeHandler(SecurityService securityService, ApplicationRepository applicationRepository)
        {
            _securityService = securityService;
            _applicationRepository = applicationRepository;
        }

        public async Task<UsersApplications> Handle(FindByCodeCommand request, CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.Find(request.Code);
            var isGlobalAdmin = _securityService.IsGlobalAdminitrator();
            UsersApplications? result = null;
            if (application == null)
            {
                if (isGlobalAdmin)
                {
                    throw new NotFoundException($"Application '{request.Code}' isn't found");
                }

                throw new ForbidenException($"Application '{request.Code}' is denied access");
            }

            var user = await _securityService.GetCurrentUser();
            result = application.Users.FirstOrDefault(a => a.UserId == user.UserId);
            if (!isGlobalAdmin && result == null)
            {
                throw new ForbidenException($"Application '{request.Code}' is denied access");
            }
            else if (isGlobalAdmin && result == null)
            {
                return new UsersApplications(await _securityService.GetCurrentUser(), application, UsersApplications.Role.Administrator);
            }
            else
            {
                return result;
            }
        }
    }
}
