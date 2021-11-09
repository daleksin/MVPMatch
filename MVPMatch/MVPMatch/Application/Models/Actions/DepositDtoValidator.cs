using FluentValidation;
using JetBrains.Annotations;
using System.Linq;

namespace MVPMatch.Application.Models.Actions
{
    [UsedImplicitly]
    public class DepositDtoValidator : AbstractValidator<DepositDto>
    {
        public DepositDtoValidator()
        {
            RuleFor(v => v.Coins)
                .Must(v => v.All(coin => Constants.SupportedCoins.Contains(coin.Key)));
        }
    }
}
