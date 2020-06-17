using FluentAssertions;
using PivotalServices.WebApiTemplate.CSharp.Features.Values;
using Xunit;

namespace PivotalServices.WebApiTemplate.CSharp.UnitTests.Features.Values
{
    public partial class PostValuesValidator
    {
        public class Given_a_PostValues_Request
        {
            private readonly PostValues.Validator validator;
            private PostValues.Request request;

            public Given_a_PostValues_Request()
            {
                validator = new PostValues.Validator();
                request = new PostValues.Request
                {
                    Param1 = "123",
                    Param2 = "1234"
                };
            }

            [Theory]
            [InlineData("1234as", false)]
            [InlineData("123456789asas", false)]
            [InlineData("123456AA", false)]
            [InlineData("", false)]
            [InlineData("123", true)]
            public void Should_validate_Param1(string value, bool isValid)
            {
                request.Param1 = value;

                validator.Validate(request).IsValid.Should().Be(isValid);
            }

            [Theory]
            [InlineData(null, false)]
            [InlineData("", false)]
            [InlineData("1234", true)]
            public void Should_validate_Param2(string value, bool isValid)
            {
                request.Param2 = value;

                validator.Validate(request).IsValid.Should().Be(isValid);
            }
        }
    }
}
