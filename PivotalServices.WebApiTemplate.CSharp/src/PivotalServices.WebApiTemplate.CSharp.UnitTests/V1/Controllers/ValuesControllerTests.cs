using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using PivotalServices.WebApiTemplate.CSharp.V1.Controllers;
using PivotalServices.WebApiTemplate.CSharp.V1.Features.Values;
using System.Threading.Tasks;
using Xunit;

namespace PivotalServices.WebApiTemplate.CSharp.UnitTests.V1.Controllers
{
    public class ValuesControllerTests
    {
        Mock<IMediator> mediator;
        public ValuesControllerTests()
        {
            mediator = new Mock<IMediator>();
        }

        [Fact]
        public void Test_IfControllerIsOfTypeControllerBase()
        {
            //Assert
            Assert.True(new ValuesController(mediator.Object) is ControllerBase);
        }

        [Fact]
        public async void Test_GetValues_Returns_CorrectResponse_If_Mediator()
        {
            //Arrange
            var request = new GetValues.Request { Values = new Values { Param1 = "12345678", Param2 = "123" } };
            mediator.Setup(m => m.Send(It.IsAny<IRequest<GetValues.Response>>(), default))
                .Returns(Task.FromResult(new GetValues.Response { Result = JsonConvert.SerializeObject(new { request.Values.Param1, request.Values.Param2 }) }));

            var controller = new ValuesController(mediator.Object);

            //Act
            var response = await controller.GetValues(request.Values.Param1, request.Values.Param2);

            //Assert
            Assert.IsType<ActionResult<GetValues.Response>>(response);
            Assert.Equal(JsonConvert.SerializeObject(new { request.Values.Param1, request.Values.Param2 }), response.Value.Result);
        }
    }
}
