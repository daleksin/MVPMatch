using Microsoft.AspNetCore.Mvc;
using MVPMatch.Application.Models.Products;
using MVPMatch.Application.Services;
using MVPMatch.Core.Models;
using MVPMatch.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVPMatch.WebApi.Controllers
{
    public class ProductsController : AuthControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly IEntityService _entityService;

        public ProductsController(
            IProductsService productsService,
            IEntityService entityService)
        {
            _productsService = productsService;
            _entityService = entityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            return await _productsService.GetAll();
        }

        [HttpPost]
        // TODO: sort out why ClaimTypes.Role is transformed into Microsoft proprietary version, for now the string will suffice 
        [ClaimRequirement("role", "seller")]
        [SellerFilter]
        public async Task<ActionResult<Product>> Create([FromBody] UpsertProductDto product)
        {
            return await _productsService.Create(product);
        }

        [HttpPut]
        [ClaimRequirement("role", "seller")]
        [SellerFilter]
        public async Task<ActionResult<Product>> Update([FromBody] UpsertProductDto product)
        {
            return await _productsService.Update(product);
        }

        [HttpDelete("{id}")]
        [SellerFilter]
        public async Task<IActionResult> Delete(int id)
        {
            await _entityService.Delete<Product>(id);

            return NoContent();
        }
    }
}
