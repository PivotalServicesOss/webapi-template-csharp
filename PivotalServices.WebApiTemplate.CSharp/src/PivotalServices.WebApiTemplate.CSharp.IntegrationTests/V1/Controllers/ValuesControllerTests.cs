using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PivotalServices.WebApiTemplate.CSharp.IntegrationTests.V1.Controllers
{
    public class ValuesControllerTests : IClassFixture<TestingWebApplicationFactory>
    {
        private readonly TestingWebApplicationFactory factory;

        public ValuesControllerTests(TestingWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task GetValues_ShouldReturnCorrectResponses()
        {
            //Arrange
            var client = factory.CreateClient();

            //Act
            var response = await client.GetAsync("/api/v1/Values/12345678/123");

            // Assert
            response.EnsureSuccessStatusCode();

            response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");

            response.Content.ReadAsStringAsync().Result.Should().Be("{\"result\":\"{\\\"Values\\\":{\\\"Param1\\\":\\\"12345678\\\",\\\"Param2\\\":\\\"123\\\"}}\"}");
        }

        [Fact]
        public async Task GetValues_ShouldReturnBadRequestResponsesOnValidationError()
        {
            //Arrange
            var client = factory.CreateClient();

            //Act
            var response = await client.GetAsync("/api/v1/Values/123/123");

            // Assert
            response.StatusCode.ToString().Should().Be("BadRequest");
            response.Content.ReadAsStringAsync().Result.Should().Be("{\"Message\":\"Validation failed: \\r\\n -- Values.Param1: Values.Param1 must be 8 digit number\"}");
        }

        [Fact]
        public async Task PostValues_ShouldReturnCorrectResponses()
        {
            //Arrange
            var client = factory.CreateClient();
            var content = new StringContent("{\"values\": {\"param1\": \"123\", \"param2\": \"123456\"}}", Encoding.UTF8, "application/json");
            
            //Act
            var response = await client.PostAsync("/api/v1/Values", content);

            // Assert
            response.EnsureSuccessStatusCode();

            response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");

            response.Content.ReadAsStringAsync().Result.Should().Be("{\"resultParam1\":\"123\",\"resultParam2\":\"123456\"}");
        }

        [Fact]
        public async Task PostValues_ShouldReturnBadRequestResponsesOnValidationError()
        {
            //Arrange
            var client = factory.CreateClient();
            var content = new StringContent("{\"values\": {\"param1\": \"1\", \"param2\": \"123456\"}}", Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("/api/v1/Values", content);

            // Assert
            response.StatusCode.ToString().Should().Be("BadRequest");
        }
    }
}
