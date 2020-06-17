using FluentAssertions;
using PivotalServices.WebApiTemplate.CSharp.V1.Features.Values;
using Xunit;

namespace PivotalServices.WebApiTemplate.CSharp.Unit.Tests.V1.Features
{
    public class GetValuesValidatorTests
    {
        private readonly GetValues.Validator validator;
        private GetValues.Request request;

        public GetValuesValidatorTests()
        {
            validator = new GetValues.Validator();
        }

        [Theory]
        [InlineData("1234as", false)]
        [InlineData("123456789asas", false)]
        [InlineData("123456AA", false)]
        [InlineData("", false)]
        [InlineData("12345678", true)]
        public void Test_ValidatorValidatesIfParam1Of8Digits(string value, bool isValid)
        {
            //Arrange
            request = new GetValues.Request
            {
                Values = new Values
                {
                    Param1 = value,
                    Param2 = "1234"
                }
            };

            //Act and Assert
            validator.Validate(request).IsValid.Should().Be(isValid);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("1234", true)]
        public void Test_ValidatorValidatesIfParam2IsNotNullAndNotEmpty(string value, bool isValid)
        {
            //Arrange
            request = new GetValues.Request
            {
                Values = new Values
                {
                    Param1 = "12345678",
                    Param2 = value
                }
            };

            //Act and Assert
            validator.Validate(request).IsValid.Should().Be(isValid);
        }
    }
}
