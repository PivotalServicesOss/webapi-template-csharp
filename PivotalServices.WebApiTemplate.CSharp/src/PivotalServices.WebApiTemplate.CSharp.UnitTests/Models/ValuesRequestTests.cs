using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using PivotalServices.WebApiTemplate.CSharp.Features;
using PivotalServices.WebApiTemplate.CSharp.Features.Values;
using Xunit;

namespace PivotalServices.WebApiTemplate.CSharp.Unit.Tests.Models
{
    public class ValuesRequestTests
    {
        [Fact]
        public void TestRequestTypes()
        {
            var request = new GetValues.Request();
            Assert.True(request is IRequest);
            Assert.True(request is IRequest<GetValues.Response>);
        }

        [Fact]
        public void TestRequestProperties()
        {
            var request = new GetValues.Request();

            Assert.NotNull(request.GetType().GetProperty("Param1"));
            Assert.Equal("String", request.GetType().GetProperty("Param1").PropertyType.Name);

            Assert.NotNull(request.GetType().GetProperty("Param2"));
            Assert.Equal("String", request.GetType().GetProperty("Param2").PropertyType.Name);
        }
    }
}
