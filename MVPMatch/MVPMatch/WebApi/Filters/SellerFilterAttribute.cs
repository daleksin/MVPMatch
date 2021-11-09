using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MVPMatch.Application.Interfaces;
using MVPMatch.Application.Models.Products;
using System.Linq;

namespace MVPMatch.WebApi.Filters
{
    public class SellerFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var productInfo = filterContext.ActionArguments.Values.OfType<UpsertProductDto>().FirstOrDefault();

            if (productInfo is not null)
            {
                var dbContext = filterContext.ActionArguments.Values.OfType<IApplicationDbContext>().FirstOrDefault();

                var existingProduct = dbContext?.Products.FirstOrDefault(p => p.ProductId == productInfo.Id);

                if (existingProduct?.SellerId !=
                    filterContext.HttpContext.RequestServices.GetService<ICurrentUserService>()?.UserId)
                {
                    //filterContext.Result = new UnauthorizedResult();
                }
            }
        }
    }
}
