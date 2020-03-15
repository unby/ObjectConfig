using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjectConfig.Data;
using ObjectConfig.Features.Configs.Create;
using ObjectConfig.Features.Configs.FindAll;
using ObjectConfig.Features.Configs.FindConfig;
using ObjectConfig.Features.Configs.JsonConverter;
using ObjectConfig.Features.Configs.Update;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Configs
{
    [Route("feature/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public partial class ConfigController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ConfigController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet("/features/application/{appCode}/environment/{envCode}/config/{confCode}")]
        [HttpGet("/features/application/{appCode}/environment/{envCode}/config/{confCode}/devenition")]
        [ProducesResponseType(typeof(List<ConfigDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConfigDevenition([FromRoute]string appCode, [FromRoute]string envCode, [FromRoute]string confCode, [FromQuery]string? versionFrom)
        {
            var result = await _mediator.Send(new FindConfigCommand(appCode, envCode, confCode, versionFrom));
            return Ok(new ConfigDto(result));
        }

        [HttpGet("/features/application/{appCode}/environment/{envCode}/configs")]
        [ProducesResponseType(typeof(List<ConfigDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConfigs(string appCode, string envCode)
        {
            var result = await _mediator.Send(
                new GetAllConfigsCommand(appCode, envCode));
            return Ok(_mapper.Map<List<Config>, List<ConfigDto>>(result));
        }

        [HttpPost("/features/application/{appCode}/environment/{envCode}/config/{confCode}")]
        [ProducesResponseType(typeof(ConfigDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateConfigs([FromRoute]string appCode, [FromRoute]string envCode,
            [FromRoute]string confCode, [FromQuery]string? versionFrom)
        {
            var result = await _mediator.Send(
                new CreateConfigCommand(appCode, envCode, confCode, await RequestBody(), versionFrom));
            return Ok(new ConfigDto(result));
        }

        [HttpPatch("/features/application/{appCode}/environment/{envCode}/config/{confCode}")]
        [ProducesResponseType(typeof(ConfigDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateEnvironment([FromRoute]string appCode,
            [FromRoute]string envCode, [FromRoute]string confCode,
            [FromQuery]string versionFrom, [FromQuery]string versionTo)
        {
            var result = await _mediator.Send(new UpdateConfigCommand(appCode, envCode, confCode, versionFrom, versionTo));
            return Ok(new ConfigDto(result));
        }

        [HttpGet("/features/application/{appCode}/environment/{envCode}/config/{confCode}/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConfigJson([FromRoute]string appCode, [FromRoute]string envCode, [FromRoute]string confCode, [FromQuery]string? versionFrom)
        {
            var result = await _mediator.Send(new JsonConverterCommand(appCode, envCode, confCode, versionFrom));
            return this.Content(result, "application/json");
        }

        private Task<string> RequestBody()
        {
            var bodyStream = new StreamReader(Request.Body);
            return bodyStream.ReadToEndAsync();
        }
    }
}
