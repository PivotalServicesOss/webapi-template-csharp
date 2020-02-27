using System.Threading;
using Newtonsoft.Json;
using PivotalServices.WebApiTemplate.CSharp.V1.Features.Values;
using Xunit;

namespace PivotalServices.WebApiTemplate.CSharp.Unit.Tests.V1.Features
{
    public class GetValuesHandlerTests
    {
        [Fact]
        public async void Test_HandleReturnsCorrectResponse()
        {
            //Arrange
            var handler = new GetValues.Handler();
            var request = new GetValues.Request { Values = new Values { Param1 = "param1", Param2 = "param2" } };

            //Act
            var response = await handler.Handle(request, new CancellationToken());

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(request), response.Result);
        }
    }
}
