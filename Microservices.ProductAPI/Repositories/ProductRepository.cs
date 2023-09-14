using AutoMapper;
using Microservices.ProductAPI.Data;
using Microservices.ProductAPI.Models;
using Microservices.ProductAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel;

namespace Microservices.ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MsDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductRepository(MsDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>?> GetAllProductsAsync()
        {
            var dbProducts = await _dbContext.Products.ToListAsync();

            if (dbProducts is null) return new List<ProductDto>();

            var productsDto = _mapper.Map<List<ProductDto>>(dbProducts);

            return productsDto;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            if (productId < 1) return null!;

            var dbProduct = await _dbContext.Products.FirstOrDefaultAsync();

            if (dbProduct is null) return null!;

            var productDto = _mapper.Map<ProductDto>(dbProduct);

            return productDto;
        }

        public async Task<bool> UpsertProductAsync(ProductDto productDto)
        {
            if (productDto is null) return false;

            var mappedProduct = _mapper.Map<ProductModel>(productDto);

            var dbProduct = await _dbContext.Products.FirstOrDefaultAsync(prod => prod.Id == mappedProduct.Id);

            EntityEntry entityEntry;
            if (dbProduct is null)
            {
                entityEntry = await _dbContext.Products.AddAsync(mappedProduct);
            }
            else
            {
                entityEntry = _dbContext.Products.Update(mappedProduct);
            }

            await _dbContext.SaveChangesAsync();

            //if product was created or updated, return true
            return entityEntry.State != EntityState.Unchanged;
        }

        public async Task<bool> DelectProductAsync(int productId)
        {
            if (productId < 1) return false;

            var dbProduct = await _dbContext.Products.FirstOrDefaultAsync(prod => prod.Id == productId);

            if (dbProduct is null) return false;

            var deletedProduct = _dbContext.Products.Remove(dbProduct);

            await _dbContext.SaveChangesAsync();

            //if product was deleted, return true
            return deletedProduct.State != EntityState.Unchanged;
        }

    }
}
