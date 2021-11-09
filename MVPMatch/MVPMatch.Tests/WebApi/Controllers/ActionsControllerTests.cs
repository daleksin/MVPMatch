using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVPMatch.Application.Interfaces;
using MVPMatch.Application.Models.Actions;
using MVPMatch.Application.Services;
using MVPMatch.WebApi.Controllers;

namespace MVPMatch.Tests.WebApi.Controllers
{
    [TestClass]
    public class ActionsControllerTests
    {
        
        [TestMethod]
        public async Task Deposit_Always_ShouldCallUpdateDeposit()
        {
            var currentUserService = A.Fake<ICurrentUserService>();
            A.CallTo(() => currentUserService.UserEmail).Returns("test@email.com");
            A.CallTo(() => currentUserService.Deposit).Returns(5);

            var identityService = A.Fake<IIdentityService>();


            var target = new ActionsController(currentUserService, identityService, A.Fake<IActionService>());
            var deposit = new DepositDto
            {
                Coins = new Dictionary<int, int>
                {
                    {5, 1},
                    {10, 2},
                }
            };

            await target.Deposit(deposit);

            A.CallTo(() => identityService.UpdateDeposit(currentUserService.UserEmail, currentUserService.Deposit + 25))
                .MustHaveHappened();
        }

        [TestMethod]
        public async Task Buy_Always_ShouldCallBuy()
        {
            var actionService = A.Fake<IActionService>();
            var target = new ActionsController(A.Fake<ICurrentUserService>(), A.Fake<IIdentityService>(), actionService);

            var param = new BuyDto();
            await target.Buy(param);

            A.CallTo(() => actionService.Buy(param)).MustHaveHappened();
        }
    }
}
