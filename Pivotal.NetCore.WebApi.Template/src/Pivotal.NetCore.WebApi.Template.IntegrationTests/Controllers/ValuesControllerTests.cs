using FluentAssertions;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pivotal.NetCore.WebApi.Template.Features;
using Pivotal.NetCore.WebApi.Template.Features.Values;
using Xunit;

namespace Pivotal.NetCore.WebApi.Template.Integration.Tests.Controllers
{
    public class ValuesControllerTests
    {
        [Fact]
        public async Task GetValues_ShouldGetSuccessStatusCode()
        {
            using (var testServer = TestHelper.GetTestServer())
            {
                var response = await testServer.CreateRequest("/api/Values/v1/12345678/123").GetAsync();
                response.IsSuccessStatusCode.Should().BeTrue();
            }
        }
        
        [Fact]
        public async Task GetValues_ShouldReturnCorrectResponse()
        {
            using (var testServer = TestHelper.GetTestServer())
            {
                var response = await testServer.CreateRequest("/api/Values/v1/12345678/123").GetAsync();
                var content = response.Content.ReadAsStringAsync();
                var formResponse = JsonConvert.DeserializeObject<GetValues.Response>(content.Result);
                formResponse.Result.Should().Be(JsonConvert.SerializeObject(new GetValues.Request { Param1 = "12345678", Param2 = "123" }));
            }
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
