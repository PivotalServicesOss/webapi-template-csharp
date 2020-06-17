using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PivotalServices.WebApiTemplate.CSharp.IntegrationTests.V1.Controllers.ValuesController
{
    public partial class ValuesControllerTests
    {
        public class Given_an_invalid_request_to_getvalues : IClassFixture<TestingWebApplicationFactory>
        {
            private HttpResponseMessage response;

            public Given_an_invalid_request_to_getvalues(TestingWebApplicationFactory factory)
            {
                var client = factory.CreateClient();
                Task.Run(() => client.GetAsync("/api/v1/Values/123/123"))
                    .ContinueWith(task => response = task.Result)
                    .Wait();
            }

            [Fact]
            public async Task Should_return_bad_request_response_on_validation_error()
            {
                response.StatusCode.ToString().Should().Be("BadRequest");
            }

            [Fact]
            public async Task Should_return_corrent_validation_errors_on_validation_error()
            {
                response.Content.ReadAsStringAsync().Result.Should().Be("{\"Message\":\"Validation failed: \\r\\n -- Param1: Param1 must be 8 digit number\"}");
            }
        }
    }
}
