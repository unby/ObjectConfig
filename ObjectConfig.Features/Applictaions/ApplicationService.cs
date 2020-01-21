using ObjectConfig.Data;
using ObjectConfig.Features.Users;
using ObjectConfig.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Applictaions
{
    public class ApplicationService
    {
        private readonly SecurityService _securityService;
        private readonly ApplicationRepository _applicationRepository;

        public ApplicationService(SecurityService securityService, ApplicationRepository applicationRepository)
        {
            _securityService = securityService;
            _applicationRepository = applicationRepository;
        }
        public async Task<List<UsersApplications>> GetApplications()
        {
            return await _applicationRepository.FindByUser((await _securityService.GetCurrentUser()).UserId);
        }

        public async Task<UsersApplications> CreateApplication(ApplicationDTO applicationDTO)
        {
            await _securityService.CheckAccess(User.Role.Administrator);

            Application application = new Application(applicationDTO.Name, applicationDTO.Code, applicationDTO.Description);
            var userApp = new UsersApplications(await _securityService.GetCurrentUser(), application, UsersApplications.Role.Administrator);
            application.Users.Add(userApp);

            await _applicationRepository.Create(application);

            return userApp;
        }
    }
}
