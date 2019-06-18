using System.Threading;
using Newtonsoft.Json;
using Pivotal.NetCore.WebApi.Template.Features;
using Pivotal.NetCore.WebApi.Template.Features.Values;
using Xunit;

namespace Pivotal.NetCore.WebApi.Template.Unit.Tests.Features
{
    public class ValuesHandlerTests
    {
        [Fact]
        public async void Test_HandleReturnsCorrectResponse()
        {
            var handler = new GetValues.Handler();
            var request = new GetValues.Request { Param1 = "param1", Param2 = "param2" };
            var response = await handler.Handle(request, new CancellationToken());

            Assert.Equal(JsonConvert.SerializeObject(request), response.Result);
        }
    }
}
