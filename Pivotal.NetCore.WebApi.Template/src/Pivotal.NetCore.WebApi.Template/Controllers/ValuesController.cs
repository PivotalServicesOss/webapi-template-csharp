using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pivotal.NetCore.WebApi.Template.Features.Values;

namespace Pivotal.NetCore.WebApi.Template.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ValuesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("v1/{param1}/{param2}")]
        public async Task<ActionResult<GetValues.Response>> GetValues(string param1, string param2)
        {
            var request = new GetValues.Request {Param1 = param1, Param2 = param2};
            return await _mediator.Send<GetValues.Response>(request);
        }
    }
}