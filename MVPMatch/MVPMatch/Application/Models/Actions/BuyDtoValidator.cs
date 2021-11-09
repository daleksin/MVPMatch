using FluentValidation;
using JetBrains.Annotations;

namespace MVPMatch.Application.Models.Actions
{
    [UsedImplicitly]
    public class BuyDtoValidator : AbstractValidator<BuyDto>
    {
        public BuyDtoValidator()
        {
            RuleFor(v => v.Amount)
                .GreaterThan(0);
            RuleFor(v => v.ProductId)
                .GreaterThan(0);
        }
    }
}
