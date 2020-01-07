using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ObjectConfig.Features.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SecurityService securityService;

        public UserController(SecurityService securityService) 
        {
            this.securityService = securityService;
        }

        public UserDto CurrentUser()
        {
            return securityService.GetCurrentUser();
        }
    }
}