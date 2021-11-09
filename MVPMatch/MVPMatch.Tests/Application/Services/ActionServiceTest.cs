using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVPMatch.Application.Interfaces;
using MVPMatch.Application.Models.Actions;
using MVPMatch.Application.Services;
using MVPMatch.Common.Exceptions;
using MVPMatch.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVPMatch.Tests.Application.Services
{
    [TestClass]
    public class ActionServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task Buy_WrongProductId_ShouldThrowNotFoundException()
        {
            var products = new List<Product>
            {
                new()
                {
                    ProductId = 1,
                    AmountAvailable = 10
                }
            }.AsQueryable();


            var fakeDbSet = A.Fake<DbSet<Product>>(builder => builder.Implements(typeof(IQueryable<Product>)));
            fakeDbSet.Initialize(products);

            var currentUserService = A.Fake<ICurrentUserService>();
            A.CallTo(() => currentUserService.Deposit).Returns(0);

            var dbContext = A.Fake<IApplicationDbContext>();
            A.CallTo(() => dbContext.Products).Returns(fakeDbSet);

            var target = CreateTarget(dbContext);

            var param = new BuyDto();
            await target.Buy(param);
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public async Task Buy_NotEnoughMoney_ShouldThrowBadRequestException()
        {
            var products = new List<Product>
            {
                new()
                {
                    ProductId = 1,
                    AmountAvailable = 10,
                    CostEur = 1
                }
            }.AsQueryable();


            var fakeDbSet = A.Fake<DbSet<Product>>(builder => builder.Implements(typeof(IQueryable<Product>)));
            fakeDbSet.Initialize(products);

            var currentUserService = A.Fake<ICurrentUserService>();
            A.CallTo(() => currentUserService.Deposit).Returns(0);

            var dbContext = A.Fake<IApplicationDbContext>();
            A.CallTo(() => dbContext.Products).Returns(fakeDbSet);

            var target = CreateTarget(dbContext);

            var param = new BuyDto { ProductId = 1, Amount = 1 };

            await target.Buy(param);
        }

        private IActionService CreateTarget(
            IApplicationDbContext applicationDbContext = default,
            ICurrentUserService currentUserService = default,
            IIdentityService identityService = default)
        {
            return A.Fake<ActionService>(
                x => x.WithArgumentsForConstructor(
                    new object[]
                    {
                        applicationDbContext ?? A.Fake<IApplicationDbContext>(),
                        currentUserService ?? A.Fake<ICurrentUserService>(),
                        identityService ?? A.Fake<IIdentityService>()

                    })
            );
        }
    }

}
