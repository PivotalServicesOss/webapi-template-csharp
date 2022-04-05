using FluentValidation;

namespace PivotalServices.WebApiTemplate.CSharp.Extensions
{
    public static class RuleBuilderExtensions
    {
        //This is a sample extension method for rule builder, so that you can add more if needed
        public static IRuleBuilderOptions<T, string> IsNonEmptyEightDigitNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.NotNull()
                .NotEmpty()
                .Matches("^[0-9]{8}$")
                .WithMessage("{PropertyName} must be 8 digit number");
        }

        public static IRuleBuilderOptions<T, string> IsNonEmptyThreeDigitNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.NotNull()
                .NotEmpty()
                .Matches("^[0-9]{3}$")
                .WithMessage("{PropertyName} must be 3 digit number");
        }
    }
}
