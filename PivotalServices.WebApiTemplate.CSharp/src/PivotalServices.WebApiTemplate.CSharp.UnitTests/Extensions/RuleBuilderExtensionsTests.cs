using FluentAssertions;
using FluentValidation;
using PivotalServices.WebApiTemplate.CSharp.Extensions;
using Xunit;

namespace PivotalServices.WebApiTemplate.CSharp.Unit.Tests.Extensions
{
    public class RuleBuilderExtensionsTests
    {
        [Theory]
        [InlineData("12345678", true)]
        [InlineData("", false)]
        [InlineData("123", false)]
        [InlineData("123456789", false)]
        public void Test_IsNonEmptyEightDigitNumber(string value, bool isValid)
        {
            //Arrange
            var validator = new EightDigitValidatorStub();

            //Act and Assert
            validator.Validate(value).IsValid.Should().Be(isValid);
        }

        [Theory]
        [InlineData("123werwere", false)]
        [InlineData("", false)]
        [InlineData("123", true)]
        [InlineData("123456789", false)]
        public void Test_IsNonEmptyThreeDigitNumber(string value, bool isValid)
        {
            //Arrange
            var validator = new ThreeDigitValidatorStub();

            //Act and Assert
            validator.Validate(value).IsValid.Should().Be(isValid);
        }
    }

    class ThreeDigitValidatorStub : AbstractValidator<string>
    {
        public ThreeDigitValidatorStub()
        {
            RuleFor(p => p).IsNonEmptyThreeDigitNumber();
        }
    }
    
    class EightDigitValidatorStub : AbstractValidator<string>
    {
        public EightDigitValidatorStub()
        {
            RuleFor(p => p).IsNonEmptyEightDigitNumber();
        }
    }

    class RequestStub
    {
        public string Param1 { get; set; }
    }
}
