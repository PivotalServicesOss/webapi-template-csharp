using FluentValidation;
using PivotalServices.WebApiTemplate.CSharp.V1.Features.Values;
using Xunit;

namespace PivotalServices.WebApiTemplate.CSharp.Unit.Tests.V1.Features
{
    public class PostValuesValidatorTests
    {
        private readonly PostValues.Validator validator;
        private PostValues.Request request;

        public PostValuesValidatorTests()
        {
            validator = new PostValues.Validator();
        }

        [Fact]
        public void Test_IfValidatorIsOfTypeAbstractValidator()
        {
            //Assert
            Assert.True(validator is AbstractValidator<PostValues.Request>);
        }

        [Theory]
        [InlineData("1234as", false)]
        [InlineData("123456789asas", false)]
        [InlineData("123456AA", false)]
        [InlineData("", false)]
        [InlineData("123", true)]
        public void Test_ValidatorValidatesIfParam1Of3Digits(string value, bool isValid)
        {
            //Arrange
            request = new PostValues.Request
            {
                Values = new Values
                {
                    Param1 = value,
                    Param2 = "1234"
                }
            };

            //Act and Assert
            Assert.Equal(isValid, validator.Validate(request).IsValid);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("1234", true)]
        public void Test_ValidatorValidatesIfParam2IsNotNullAndNotEmpty(string value, bool isValid)
        {
            //Arrange
            request = new PostValues.Request
            {
                Values = new Values
                {
                    Param1 = "123",
                    Param2 = value
                }
            };

            //Act and Assert
            Assert.Equal(isValid, validator.Validate(request).IsValid);
        }
    }
}
