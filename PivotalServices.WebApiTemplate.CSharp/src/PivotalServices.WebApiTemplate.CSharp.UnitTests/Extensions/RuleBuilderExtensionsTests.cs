using FluentValidation;
using PivotalServices.WebApiTemplate.CSharp.Extensions;
using Xunit;

namespace PivotalServices.WebApiTemplate.CSharp.Unit.Tests.Extensions
{
    public class RuleBuilderExtensionsTests
    {
        [Fact]
        public void Test_IsNonEmptyEightDigitNumber()
        {
            //Arrange
            var validator = new EightDigitValidatorStub();

            //Act and Assert
            Assert.True(validator.Validate("12345678").IsValid);
            Assert.False(validator.Validate(string.Empty).IsValid);
            Assert.False(validator.Validate("123").IsValid);
            Assert.False(validator.Validate("123456789").IsValid);
        }

        [Fact]
        public void Test_IsNonEmptyThreeDigitNumber()
        {
            //Arrange
            var validator = new ThreeDigitValidatorStub();

            //Act and Assert
            Assert.True(validator.Validate("123").IsValid);
            Assert.False(validator.Validate(string.Empty).IsValid);
            Assert.False(validator.Validate("123werwere").IsValid);
            Assert.False(validator.Validate("123456789").IsValid);
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
