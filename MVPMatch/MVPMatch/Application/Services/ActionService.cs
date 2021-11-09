using MVPMatch.Application.Interfaces;
using MVPMatch.Application.Models.Actions;
using MVPMatch.Common.Exceptions;
using MVPMatch.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MVPMatch.Application.Services
{
    public interface IActionService
    {
        Task<BuyResultDto> Buy(BuyDto buyInformation);
    }

    public class ActionService : IActionService
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public ActionService(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }
        public async Task<BuyResultDto> Buy(BuyDto buyInformation)
        {
            var existingProduct =
                _applicationDbContext.Products.FirstOrDefault(c => c.ProductId == buyInformation.ProductId)
                ?? throw new NotFoundException(nameof(Product), buyInformation.ProductId);

            // Transactions are not supported for the InMemory database, but it would be enabled for the real one
            //await using (var transaction = await _applicationDbContext.Database.BeginTransactionAsync())
            {
                var totalPrice = existingProduct.CostEur * buyInformation.Amount;

                if (_currentUserService.Deposit < existingProduct.CostEur * buyInformation.Amount)
                {
                    throw new BadRequestException("Not enough money on the account");
                }

                await _identityService.UpdateDeposit(_currentUserService.UserEmail, _currentUserService.Deposit - totalPrice);

                existingProduct.AmountAvailable -= buyInformation.Amount;
                await _applicationDbContext.SaveChangesAsync();
                //await transaction.CommitAsync();

                return new BuyResultDto
                {
                    Change = CalculateChange(_currentUserService.Deposit),
                    Product = existingProduct,
                    MoneySpentEur = totalPrice,
                    Amount = buyInformation.Amount
                };
            }
        }

        public virtual ChangeDto CalculateChange(decimal amount)
        {
            var result = new ChangeDto();

            foreach (var coin in Constants.SupportedCoins.OrderByDescending(c => c).Where(c => c <= amount))
            {
                var coinCount = (int) (amount / coin);
                if (coinCount > 0)
                {
                    result.Coins[coin] = coinCount;
                }

                amount -= coinCount * coin;
            }

            return result;
        }
    }
}
