using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjectConfig.Data;
using ObjectConfig.Features.Applictaions.Create;
using ObjectConfig.Features.Applictaions.FindByCode;
using ObjectConfig.Features.Applictaions.Update;
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
        private readonly IMediator _mediator;

        public ApplicationController(IMapper mapper, IMediator mediator, ApplicationService applicationService)
        {
            _mapper = mapper;
            _mediator = mediator;
            _applicationService = applicationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ApplicationDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetApplications()
        {
            return Ok(_mapper.Map<List<UsersApplications>, List<ApplicationDTO>>(await _applicationService.GetApplications()));
        }

        [HttpGet("{code}")]
        [ProducesResponseType(typeof(ApplicationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetApplication(string code)
        {
            var response = await _mediator.Send(new FindByCodeCommand(code));
            var rr = _mapper.Map<UsersApplications, ApplicationDTO>(response);
            return Ok(rr);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApplicationDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateApplication(CreateApplicationDto application)
        {
            var app = await _mediator.Send(new CreateCommand(application.Name, application.Code, application.Description));
            var response = new ApplicationDTO(app);
            return Ok(response);
        }

        [HttpPatch("{code}/update")]
        [ProducesResponseType(typeof(ApplicationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateApplication([FromRoute]string code, [FromBody] UpdateApplicationDto application)
        {
            var app = await _mediator.Send(new UpdateCommand(code, application));
            var response = new ApplicationDTO(app);
            return Ok(response);
        }
    }
}