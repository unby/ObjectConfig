using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjectConfig.Data;
using ObjectConfig.Features.Configs.Create;
using ObjectConfig.Features.Configs.FindAll;
using ObjectConfig.Features.Configs.FindByCode;
using ObjectConfig.Features.Configs.Update;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Configs
{
    [Route("feature/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ConfigController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet("/features/application/{appCode}/environment/{envCode}/config/{confCode}")]
        [ProducesResponseType(typeof(List<ConfigDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConfig([FromRoute]string appCode, [FromRoute]string envCode, [FromRoute]string confCode)
        {
            var result = await _mediator.Send(new FindConfigCommand(appCode, envCode, confCode, null, null));
            return Ok(new ConfigDto(result));
        }

        [HttpGet("/features/application/{appCode}/environment/{envCode}/configs")]
        [ProducesResponseType(typeof(List<ConfigDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetConfigs(string appCode, string envCode)
        {
            var result = await _mediator.Send(
                new GetAllConfigsCommand(appCode, envCode));
            return Ok(_mapper.Map<List<Config>, List<ConfigDto>>(result));
        }

        [HttpPost("/features/application/{appCode}/environment/{envCode}/config/{confCode}")]
        [ProducesResponseType(typeof(ConfigDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateConfigs([FromRoute]string appCode, [FromRoute]string envCode,
            [FromRoute]string confCode, [FromBody]CreateConfigDefenitionDto config)
        {
            var result = await _mediator.Send(
                new CreateConfigCommand(appCode, envCode, config.Name, config.Code, config.Description));
            return Ok(new ConfigDto(result));
        }

        [HttpPatch("/features/application/{appCode}/environment/{envCode}/config/{confCode}")]
        [ProducesResponseType(typeof(ConfigDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEnvironment([FromRoute]string appCode, [FromRoute]string envCode, [FromRoute]string confCode)
        {
            var result = await _mediator.Send(new UpdateConfigCommand(appCode, envCode, confCode, null, null));
            return Ok(new ConfigDto(result));
        }
    }
}
