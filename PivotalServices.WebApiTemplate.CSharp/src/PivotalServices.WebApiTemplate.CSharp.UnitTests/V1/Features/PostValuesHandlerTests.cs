using System.Threading;
using PivotalServices.WebApiTemplate.CSharp.V1.Features.Values;
using Xunit;

namespace PivotalServices.WebApiTemplate.CSharp.Unit.Tests.V1.Features
{
    public class PostValuesHandlerTests
    {
        [Fact]
        public async void Test_HandleReturnsCorrectResponse()
        {
            //Arrange
            var handler = new PostValues.Handler();
            var request = new PostValues.Request { Values = new Values { Param1 = "param1", Param2 = "param2" } };

            //Act
            var response = await handler.Handle(request, new CancellationToken());

            //Assert
            Assert.Equal("param1", response.ResultParam1);
            Assert.Equal("param2", response.ResultParam2);
        }
    }
}
