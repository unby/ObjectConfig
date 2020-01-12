using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjectConfig.Features.Users;

namespace ObjectConfig.Features.Applictaions
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplictaionController : ControllerBase
    {
        private readonly SecurityService securityService;

        public ApplictaionController(SecurityService securityService) 
        {
            this.securityService = securityService;
        }

        [HttpGet]
        public ApplicationDTO GetApplications()
        {
            return null;
        }
    }
}