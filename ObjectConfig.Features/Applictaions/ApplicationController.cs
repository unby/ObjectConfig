using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ObjectConfig.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Applictaions
{
    [Route("feature/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationService _applicationService;
        private readonly IMapper _mapper;

        public ApplicationController(IMapper mapper, ApplicationService applicationService)
        {
            this._applicationService = applicationService;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetApplications()
        {
            return Ok(_mapper.Map<List<UsersApplications>, List<ApplicationDTO>>(await _applicationService.GetApplications()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateApplications(ApplicationDTO application)
        {
            return Ok(_mapper.Map<UsersApplications, ApplicationDTO>(await _applicationService.CreateApplication(application)));
        }
    }
}