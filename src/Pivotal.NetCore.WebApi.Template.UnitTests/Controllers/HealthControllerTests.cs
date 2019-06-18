using Pivotal.NetCore.WebApi.Template.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Steeltoe.CloudFoundry.Connector.Relational;
using Steeltoe.Common.HealthChecks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using Xunit;

namespace Pivotal.NetCore.WebApi.Template.Unit.Tests.Controllers
{
    public class HealthControllerTests
    {
        Mock<IServiceProvider> _serviceProvider;
        private Mock<IDbConnection> _dbConnection;
        private Mock<ILogger<HealthController>> _logger;

        public HealthControllerTests()
        {
            _serviceProvider = new Mock<IServiceProvider>();
            _dbConnection = new Mock<IDbConnection>();
            _logger = new Mock<ILogger<HealthController>>();
        }

        [Fact]
        public void Test_IfConstructorSetsMemberContributors()
        {
            _serviceProvider.Setup(x => x.GetService(typeof(IEnumerable<IHealthContributor>))).Returns(
                new List<IHealthContributor>()
                {
                    new RelationalHealthContributor(_dbConnection.Object)
                });
            var controller = new HealthController(_serviceProvider.Object, _logger.Object);
            var contributors = controller.healthContributors;
            Assert.Single(contributors);
            Assert.True(contributors[0] is RelationalHealthContributor);
        }

        [Fact]
        public void Test_IfControllerReturnsInternalServerErrorEvenIfOneOfTheServicesIsDown()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IHealthContributor>(new HealthContributorStub("healthy1", true));
            services.AddSingleton<IHealthContributor>(new HealthContributorStub("unhealthy2", false));
            services.AddSingleton<IHealthContributor>(new HealthContributorStub("healthy3", true));

            var controller = new HealthController(services.BuildServiceProvider(), _logger.Object);
            var result = controller.Get();
            Assert.True(result is ObjectResult);
            Assert.Equal((int)HttpStatusCode.InternalServerError, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public void Test_IfControllerReturnsOkIfAllOfTheServicesAreUp()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IHealthContributor>(new HealthContributorStub("healthy1", true));
            services.AddSingleton<IHealthContributor>(new HealthContributorStub("healthy2", true));
            services.AddSingleton<IHealthContributor>(new HealthContributorStub("healthy3", true));

            var controller = new HealthController(services.BuildServiceProvider(), _logger.Object);
            var result = controller.Get();
            Assert.True(result is OkObjectResult);
            Assert.Equal((int)HttpStatusCode.OK, ((ObjectResult)result).StatusCode);
        }
    }

    internal class HealthContributorStub : IHealthContributor
    {
        private readonly bool isHealthy;

        public HealthContributorStub(string id, bool isHealthy)
        {
            Id = id;
            this.isHealthy = isHealthy;
        }

        public HealthCheckResult Health()
        {
            return isHealthy ? new HealthCheckResult { Status = HealthStatus.UP } : new HealthCheckResult { Status = HealthStatus.DOWN };
        }

        public string Id { get; }
    }
}
