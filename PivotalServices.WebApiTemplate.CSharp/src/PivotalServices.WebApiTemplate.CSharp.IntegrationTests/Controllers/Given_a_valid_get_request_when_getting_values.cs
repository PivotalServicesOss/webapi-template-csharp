using System.Net.Http;
using FluentAssertions;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PivotalServices.WebApiTemplate.CSharp.Features.Values;
using Xunit;

namespace PivotalServices.WebApiTemplate.CSharp.Integration.Tests.Controllers
{
    public class Given_a_valid_get_request_when_getting_values
    {
        private HttpResponseMessage _response;

        public Given_a_valid_get_request_when_getting_values()
        {
            using (var testServer = TestHelper.GetTestServer())
            {
                Task.Run(() => testServer.CreateRequest("/api/Values/v1/12345678/123").GetAsync())
                    .ContinueWith(x => _response = x.Result)
                    .Wait();
            }
        }
        
        [Fact]
        public void Should_get_success_statuscode()
        {
            _response.IsSuccessStatusCode.Should().BeTrue();
        }
        
        [Fact]
        public async Task GetValues_ShouldReturnCorrectResponse()
        {
            var content = await _response.Content.ReadAsStringAsync();
            var formResponse = JsonConvert.DeserializeObject<GetValues.Response>(content);
            formResponse.Result.Should().Be(JsonConvert.SerializeObject(new GetValues.Request { Param1 = "12345678", Param2 = "123" }));
        }
        
        [Fact]
        public async Task GetValues_ShouldReturnInvalidResponse()
        {
            using (var testServer = TestHelper.GetTestServer())
            {
                var response = await testServer.CreateRequest("/api/Values/v1/asdf123/123").GetAsync();
                response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            }
        }
    }
}
