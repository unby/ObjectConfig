using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ObjectConfig.Data;
using ObjectConfig.Features.Users;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Applictaions
{
    [Route("feature/[controller]")]
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
        public async Task<IActionResult> CurrentUser()
        {
            return Ok(_mapper.Map<User, UserDto>(await _securityService.GetCurrentUser()));
        }
    }
}