using FluentValidation;
using Pivotal.NetCore.WebApi.Template.Extensions;
using Xunit;

namespace Pivotal.NetCore.WebApi.Template.Unit.Tests.Extensions
{
    public class RuleBuilderExtensionsTests
    {
        [Fact]
        public void Test_IsNonEmptyEightDigitNumber()
        {
            var validator = new ValidatorStub();
            Assert.True(validator.Validate("12345678").IsValid);
            Assert.False(validator.Validate(string.Empty).IsValid);
            Assert.False(validator.Validate("123").IsValid);
            Assert.False(validator.Validate("123456789").IsValid);
        }
    }

    class ValidatorStub : AbstractValidator<string>
    {
        public ValidatorStub()
        {
            RuleFor(p => p).IsNonEmptyEightDigitNumber();
        }
    }

    class RequestStub
    {
        public string Param1 { get; set; }
    }
}
