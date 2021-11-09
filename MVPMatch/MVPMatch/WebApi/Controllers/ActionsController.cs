using Microsoft.AspNetCore.Mvc;
using MVPMatch.Application.Interfaces;
using MVPMatch.Application.Models.Actions;
using MVPMatch.Application.Services;
using MVPMatch.WebApi.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace MVPMatch.WebApi.Controllers
{
    public class ActionsController : AuthControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private readonly IActionService _actionService;

        public ActionsController(
            ICurrentUserService currentUserService,
            IIdentityService identityService,
            IActionService actionService)
        {
            _currentUserService = currentUserService;
            _identityService = identityService;
            _actionService = actionService;
        }

        [HttpPost]
        [Route("deposit")]
        // TODO: sort out why ClaimTypes.Role is transformed into Microsoft proprietary version, for now the string will suffice
        [ClaimRequirement("role", "buyer")]
        public async Task<ActionResult<decimal>> Deposit([FromBody] DepositDto deposit)
        {
            return await _identityService.UpdateDeposit(_currentUserService.UserEmail, _currentUserService.Deposit + deposit.Coins.Sum(c => c.Key * c.Value));
        }

        [HttpPost]
        [Route("buy")]
        [ClaimRequirement("role", "buyer")]
        public async Task<ActionResult<BuyResultDto>> Buy([FromBody] BuyDto order)
        {
            return await _actionService.Buy(order);
        }

        [HttpPost]
        [Route("reset")]
        [ClaimRequirement("role", "buyer")]
        public async Task<ActionResult> Reset()
        {
            await _identityService.UpdateDeposit(_currentUserService.UserEmail, 0);

            return NoContent();
        }
    }
}
