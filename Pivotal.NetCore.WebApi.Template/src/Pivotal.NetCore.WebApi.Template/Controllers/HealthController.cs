using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Endpoint.Health;

namespace Pivotal.NetCore.WebApi.Template.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[Controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        public readonly IList<IHealthContributor> healthContributors;
        private readonly ILogger<HealthController> logger;

        public HealthController(IServiceProvider serviceProvider, ILogger<HealthController> logger)
        {
            this.healthContributors = (serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider))).GetServices<IHealthContributor>().ToList();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("v1/healthcheck")]
        public IActionResult Get()
        {
            try
            {
                var data = new List<string>
                {
                    $"Contributors Count: {healthContributors.Count}"
                };
                var overallHealth = new DefaultHealthAggregator().Aggregate(healthContributors);
                data.Add($"Status: {overallHealth.Status}");
                data.Add($"Description: {overallHealth.Description}");
                foreach (var detail in overallHealth.Details)
                    data.Add($"{detail.Key}: {JsonConvert.SerializeObject(detail.Value)}");
                if (overallHealth.Status == HealthStatus.UP)
                    return new OkObjectResult(data);

                return StatusCode((int)HttpStatusCode.InternalServerError, data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return StatusCode((int) HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}