using FluentValidation;
using JetBrains.Annotations;

namespace MVPMatch.Application.Models.Products
{
    [UsedImplicitly]
    public class UpdateProductDtoValidator : AbstractValidator<UpsertProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(v => v.Id)
                .GreaterThanOrEqualTo(0);
            RuleFor(v => v.ProductName)
                .NotEmpty();
            RuleFor(v => v.AmountAvailable)
                .GreaterThanOrEqualTo(0);
            RuleFor(v => v.CostEur)
                .GreaterThan(0);
        }
    }
}
