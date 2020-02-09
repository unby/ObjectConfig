using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjectConfig.Data;
using ObjectConfig.Features.Environments.Create;
using ObjectConfig.Features.Environments.FindAll;
using ObjectConfig.Features.Environments.FindByCode;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Environments
{
    [Route("feature/[controller]")]
    [ApiController]
    public class EnvironmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public EnvironmentController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet("/features/application/{appCode}/environments")]
        [ProducesResponseType(typeof(List<EnvironmentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEnvironments(string appCode)
        {
            var result = await _mediator.Send(new GetAllEnvironmentsCommand(appCode));
            return Ok(_mapper.Map<List<UsersEnvironments>, List<EnvironmentDto>>(result));
        }

        [HttpGet("/features/application/{appCode}/environment/{envCode}")]
        [ProducesResponseType(typeof(EnvironmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEnvironment(string appCode, string envCode)
        {
            var result = await _mediator.Send(new FindByCodeEnvironmentCommand(appCode, envCode));
            return Ok(new EnvironmentDto(result));
        }

        [HttpPost("/features/application/{appCode}/environment/")]
        [ProducesResponseType(typeof(EnvironmentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateEnvironment(string appCode, [FromBody]CreateEnvironmentDto environment)
        {
            var result = await _mediator.Send(new CreateEnvironmentsCommand(appCode, environment.Name, environment.Code, environment.Description));
            return Ok(new EnvironmentDto(result));
        }

        [HttpPatch("{code}/update")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEnvironment([FromRoute]string code, [FromBody] EnvironmentDto application)
        {
            //var app = await _mediator.Send(new UpdateCommand(code, application));
            //var response = new ApplicationDTO(app);
            return Ok(null);
        }
    }
}