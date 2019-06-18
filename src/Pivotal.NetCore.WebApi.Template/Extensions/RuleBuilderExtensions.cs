using FluentValidation;

namespace Pivotal.NetCore.WebApi.Template.Extensions
{
    public static class RuleBuilderExtensions
    {
        //This is a sample extension method for rule builder, so that you can add more if needed
        public static IRuleBuilderOptions<T, string> IsNonEmptyEightDigitNumber<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.NotNull()
                .NotEmpty()
                .Matches("^[0-9]{8}$").WithMessage("Param1 must be 8 digit number");
        }
    }
}
