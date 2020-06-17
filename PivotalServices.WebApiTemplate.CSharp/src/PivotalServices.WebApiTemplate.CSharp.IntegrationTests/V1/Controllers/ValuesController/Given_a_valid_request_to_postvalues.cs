using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PivotalServices.WebApiTemplate.CSharp.IntegrationTests.V1.Controllers.ValuesController
{
    public partial class ValuesControllerTests
    {
        public class Given_a_valid_request_to_postvalues : IClassFixture<TestingWebApplicationFactory>
        {
            private HttpResponseMessage response;

            public Given_a_valid_request_to_postvalues(TestingWebApplicationFactory factory)
            {
                var content = new StringContent("{\"param1\": \"123\", \"param2\": \"123456\"}", Encoding.UTF8, "application/json");

                var client = factory.CreateClient();

                Task.Run(() => client.PostAsync("/api/v1/Values", content))
                    .ContinueWith(task => response = task.Result)
                    .Wait();
            }

            [Fact]
            public async Task Should_be_success()
            {
                response.EnsureSuccessStatusCode();
            }

            [Fact]
            public async Task Should_have_correct_headers()
            {
                response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");
            }

            [Fact]
            public async Task Should_have_correct_response()
            {
                response.Content.ReadAsStringAsync().Result.Should().Be("{\"resultParam1\":\"123\",\"resultParam2\":\"123456\"}");
            }
        }
    }
}
