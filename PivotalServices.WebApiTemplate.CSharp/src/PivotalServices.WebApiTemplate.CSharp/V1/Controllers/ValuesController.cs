using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PivotalServices.WebApiTemplate.CSharp.V1.Features.Values;

namespace PivotalServices.WebApiTemplate.CSharp.V1.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMediator mediator;

        public ValuesController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Sends a combined json object of the 2 input parameters
        /// </summary>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        [HttpGet]
        [Route("{param1}/{param2}")]
        public async Task<ActionResult<GetValues.Response>> GetValues(string param1, string param2)
        {
            var request = new GetValues.Request { Values = new Values{ Param1 = param1, Param2 = param2 } };
            return await mediator.Send<GetValues.Response>(request);
        }

        /// <summary>
        /// Sends the same object back as response json object
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Values
        ///     {
        ///         "values": {
        ///             "param1": "string",
        ///             "param2": "string"
        ///             }
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        [HttpPost]
        public async Task<ActionResult<PostValues.Response>> PostValues(PostValues.Request request)
        {
            return await mediator.Send<PostValues.Response>(request);
        }
    }
}