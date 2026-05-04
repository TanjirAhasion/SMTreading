using Microsoft.EntityFrameworkCore;
using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Items;
using SMT.Infrastructure.Common;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Repositories.Items
{
    public class ProductSerialRepository(AppDbContext db) :BaseRepository<ProductSerial>(db), IProductSerialRepository
    {
        public async Task<string> GenerateUniqueSerialAsync(string modelNumber)
        {
            string serial;
            do
            {
                serial = GeneratedProductSerial.Generate(modelNumber);
            }
            while (await IsSerialNumberExistsAsync(serial));

            return serial;
        }

        public async Task<List<ProductSerial>> GetAllWithProductsAsync()
        {
            return await db.ProductSerials
                .Include(p => p.Product)
                .ThenInclude(p => p.Brand)
                .ToListAsync();
        }

       

        public async Task<PagedResult<ProductSerialDto>> GetPagedAsync(
     int page,
     int pageSize,
     string? search)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;   
            search = search?.Trim();

            var baseQuery = db.ProductSerials
                .AsNoTracking()
                .Where(ps =>
                    string.IsNullOrEmpty(search) ||
                    EF.Functions.Like(ps.SerialNumber, $"%{search}%")
                );

            // Optional: filter only available stock
            // baseQuery = baseQuery.Where(ps => ps.Status == ProductSerialStatus.InStock);

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .OrderByDescending(ps => ps.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ps => new ProductSerialDto(
                    ps.Id,
                    ps.SerialNumber,
                    ps.ProductId,
                    ps.Product.Name,
                    ps.Product.Model,
                    ps.Product.Brand != null ? ps.Product.Brand.Name : "",
                    ps.Status.ToString(),
                    ps.PurchaseCost,
                    ps.SellingCost,
                    ps.RentalCost,
                    ps.IsSerialNumberLinkToProduct,
                    ps.LinkedProductSerialNumberImageUrl
                ))
                .ToListAsync();

            return new PagedResult<ProductSerialDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<string> GetProductSerialAsync(long id)
        {
            var productSerial = await db.ProductSerials
                .Where(ps => ps.Id == id)
                .FirstOrDefaultAsync();
            return productSerial?.SerialNumber ?? string.Empty;
        }

        public async Task<ProductSerial?> GetProductSerialWithSerialNumber(string serialNumber)
        {
            return await db.ProductSerials
                .Where(ps => ps.SerialNumber == serialNumber)
                .FirstOrDefaultAsync();
        }

        public Task<bool> IsSerialNumberExistsAsync(string serialNumber)
        {
           return db.ProductSerials.AnyAsync(ps => ps.SerialNumber == serialNumber);
        }

        public async Task<bool> UpdateProducSerialLinkedStatusWithImage(long id, string linkedURL)
        {
           var result = await db.ProductSerials.Where(ps => ps.Id == id)
                .ExecuteUpdateAsync(ps => ps
                    .SetProperty(p => p.IsSerialNumberLinkToProduct, !string.IsNullOrEmpty(linkedURL))
                    .SetProperty(p => p.LinkedProductSerialNumberImageUrl, linkedURL));
           return result > 0;
        }

        public async Task<List<ProductSerial>> GetBySerialNumbersAsync(List<string> serials)
        {
            var distinctSerials = serials
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Distinct()
                .ToList();

            return await db.ProductSerials
                .Where(ps => distinctSerials.Contains(ps.SerialNumber))
                .ToListAsync();
        }

        public async Task UpdateRangeAsync(List<ProductSerial> serials)
        {
            db.ProductSerials.UpdateRange(serials);
            await db.SaveChangesAsync();
        }
    }
}
