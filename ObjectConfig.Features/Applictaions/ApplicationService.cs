using AutoMapper;
using ObjectConfig.Data;
using ObjectConfig.Features.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectConfig.Features.Applictaions
{
    public class ApplicationService
    {
        private readonly SecurityService _securityService;

        public ApplicationService(SecurityService  securityService) 
        {
            _securityService = securityService;
        }
        public List<Application> GetApplications() 
        {
            return null;
        }
    }
}
