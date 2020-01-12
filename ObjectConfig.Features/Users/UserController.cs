using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjectConfig.Data;
using ObjectConfig.Features.Users;

namespace ObjectConfig.Features.Applictaions
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SecurityService _securityService;
        private readonly IMapper _mapper;

        public UserController(SecurityService securityService, IMapper mapper) 
        {
            _securityService = securityService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult CurrentUser()
        {
            return Ok(_mapper.Map<User, UserDto>(_securityService.GetCurrentUser()));
        }
    }
}