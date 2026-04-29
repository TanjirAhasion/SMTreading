using Microsoft.EntityFrameworkCore;
using SMT.Application.DTO.Items;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Repositories.Items
{
    public class ProductService(IProductRepository repo) : IProductService
    {
        public async Task<List<ProductDto>> GetAllAsync() =>
            (await repo.GetAllWithBrandsAsync()).Select(x => new ProductDto(x.Id, x.Name, x.Model, x.BrandId, x.Brand.Name, x.DefaultSalePrice, x.DefaultRentPrice,x.LowStockThreshold, x.IsActive)).ToList();
      
        public async Task<ProductDto?> GetByIdAsync(long id)
        {
            var x = await repo.GetByIdAsync(id);
            return x is null ? null : new ProductDto(x.Id, x.Name, x.Model, x.BrandId, x.Brand.Name, x.DefaultSalePrice, x.DefaultRentPrice, x.LowStockThreshold, x.IsActive);
        }

        public async Task<long> CreateAsync(CreateProductDto dto)
        {
            var product = new Product { Name = dto.Name, Model = dto.Model, BrandId = dto.BrandId, DefaultSalePrice = dto.DefaultSalePrice, DefaultRentPrice = dto.DefaultRentPrice, LowStockThreshold = dto.LowStockThreshold, Description = dto.Description, Brand = default! };
            await repo.CreateAsync(product);
            return product.Id;
            //return new ProductDto(entity.Id, entity.Name, entity.Model, entity.BrandId, entity.Brand.Name, entity.DefaultSalePrice, entity.DefaultRentPrice, entity.LowStockThreshold, entity.IsActive);
        }

        public async Task<long?> UpdateAsync(long id, UpdateProductDto dto)
        {
            var entity = await repo.GetByIdAsync(id);
            if (entity is null) return null;
            entity.Name = dto.Name; entity.Model = dto.Model; entity.BrandId = dto.BrandId; entity.DefaultSalePrice = dto.DefaultSalePrice; entity.DefaultRentPrice = dto.DefaultRentPrice; entity.LowStockThreshold = dto.LowStockThreshold; entity.Description = dto.Description; entity.IsActive = dto.IsActive;
            await repo.UpdateAsync(entity);
            return entity.Id;
        }

        public Task<bool> DeleteAsync(long id) => repo.DeleteAsync(id);
    }
}
