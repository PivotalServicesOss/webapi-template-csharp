using Pivotal.NetCore.WebApi.Template.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pivotal.NetCore.WebApi.Template.Features;
using Pivotal.NetCore.WebApi.Template.Features.Values;
using Xunit;

namespace Pivotal.NetCore.WebApi.Template.Unit.Tests.Controllers
{
    public class ValuesControllerTests
    {
        Mock<IMediator> mediator;

        public ValuesControllerTests()
        {
            mediator = new Mock<IMediator>();
        }

        [Fact]
        public async void Test_GetValues_AccecptsTypeOfValuesRequest()
        {
            var controller = new ValuesController(mediator.Object);

            mediator.Setup(m => m.Send<GetValues.Response>(It.IsAny<GetValues.Request>(), It.IsAny<CancellationToken>()))
                .Returns(Task.Run(()=> { return new GetValues.Response(); }));

            var response = await controller.GetValues( "123", "234" );

            Assert.True(response.Value is GetValues.Response);
        }

        [Fact]
        public void Test_ControllerAttributes()
        {
            var controller = new ValuesController(mediator.Object);

            var attributes = Attribute.GetCustomAttributes(controller.GetType()).ToList();

            Assert.Equal(3, attributes.Count);
            Assert.Contains(attributes, a =>a is RouteAttribute);
            Assert.Contains(attributes, a =>a is ControllerAttribute);
            Assert.Contains(attributes, a =>a is ApiControllerAttribute);

            var routeAttribute = (RouteAttribute)attributes.First(a => a is RouteAttribute);
            Assert.Equal("api/[Controller]", routeAttribute.Template);
        }

        [Fact]
        public void Test_ControllerGetValuesMethodAttributes()
        {
            var controller = new ValuesController(mediator.Object);

            var members = controller.GetType().GetMembers().ToList();

            var getValueMember = members.First(a => a.Name == "GetValues");

            var attributes = Attribute.GetCustomAttributes(getValueMember).ToList();

            var routeAttribute = (RouteAttribute)attributes.First(a => a is RouteAttribute);
            Assert.Equal("v1/{param1}/{param2}", routeAttribute.Template);

            Assert.Contains(attributes, a => a is HttpGetAttribute);
        }
    }
}
