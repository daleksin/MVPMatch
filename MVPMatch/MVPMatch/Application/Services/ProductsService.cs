using Microsoft.EntityFrameworkCore;
using MVPMatch.Application.Interfaces;
using MVPMatch.Application.Models.Products;
using MVPMatch.Common.Exceptions;
using MVPMatch.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVPMatch.Application.Services
{
    public interface IProductsService
    {
        Task<List<Product>> GetAll();
        Task<Product> Create(UpsertProductDto product);
        Task<Product> Update(UpsertProductDto product);
    }

    public class ProductsService : IProductsService
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;

        public ProductsService(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _applicationDbContext.Products.ToListAsync();
        }

        public async Task<Product> Create(UpsertProductDto productDto)
        {
            var product = new Product
            {
                AmountAvailable = productDto.AmountAvailable,
                CostEur = productDto.CostEur,
                ProductName = productDto.ProductName,
                SellerId = _currentUserService.UserId
            };

            _applicationDbContext.Products.Add(product);
            await _applicationDbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> Update(UpsertProductDto product)
        {
            var existingProduct =
                await _applicationDbContext.Products.FirstOrDefaultAsync(c => c.ProductId== product.Id)
                ?? throw new NotFoundException(nameof(Product), product.Id);

            existingProduct.AmountAvailable = product.AmountAvailable;
            existingProduct.CostEur = product.CostEur;
            existingProduct.ProductName = product.ProductName;
            
            await _applicationDbContext.SaveChangesAsync();

            return existingProduct;
        }
    }
}
