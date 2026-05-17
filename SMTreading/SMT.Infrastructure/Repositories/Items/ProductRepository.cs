using Microsoft.EntityFrameworkCore;
using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Items;
using SMT.Domain.Enums;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Repositories.Items
{
    public class ProductRepository(AppDbContext db) : BaseRepository<Product>(db), IProductRepository
    {
        public async Task<List<Product>> GetAllWithBrandsAsync()
        {
            return await db.Products
                .Include(p => p.Brand)
                .ToListAsync();
        }

        public async Task<List<Product>> GetByIdsAsync(List<long> productIds)
        {
            return await db.Products.Include(x=>x.Brand)
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }

        public async Task<PagedResult<ProductListDto>> GetPagedAsync(int page, int pageSize, string? search, int? status)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;
            search = search?.Trim();

            var baseQuery = db.Products
                .Include(p => p.Brand)
                .AsNoTracking()
                .Where(ps =>
                    string.IsNullOrEmpty(search) ||
                    EF.Functions.Like(ps.Name, $"%{search}%") || 
                    EF.Functions.Like(ps.Model, $"%{search}%")
                );

            // Optional: filter only available stock
            //if (status != null)
            //    baseQuery = baseQuery.Where(ps => ps.Status == (ProductSerialStatus)status);

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductListDto(
                    p.Id,
                    p.Name,
                    p.Model,
                    p.BrandId,
                    p.Brand != null ? p.Brand.Name : "",
                    p.DefaultSalePrice,
                    p.DefaultRentPrice,
                    p.LowStockThreshold,                    
                    db.ProductSerials.Count(ps => ps.ProductId == p.Id && ps.Status == ProductSerialStatus.InStock), // InStock
                    db.ProductSerials.Count(ps => ps.ProductId == p.Id && ps.Status == ProductSerialStatus.InRent), // InRent
                    p.IsActive
                    )).ToListAsync();

            return new PagedResult<ProductListDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
           
