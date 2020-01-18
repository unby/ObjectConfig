using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ObjectConfig.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Applictaions
{
    [Route("feature/[controller]")]
    [ApiController]
    public class ApplictaionController : ControllerBase
    {
        private readonly ApplicationService _applicationService;
        private readonly IMapper _mapper;

        public ApplictaionController(IMapper mapper, ApplicationService applicationService)
        {
            this._applicationService = applicationService;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetApplications()
        {
            return Ok(_mapper.Map<List<UsersApplications>, List<ApplicationDTO>>(await _applicationService.GetApplications()));
        }
    }
}