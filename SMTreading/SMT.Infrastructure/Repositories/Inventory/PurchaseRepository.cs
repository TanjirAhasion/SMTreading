using SMT.Application.DTO.Inventory;
using SMT.Application.Helper;
using SMT.Application.Interfaces.Inventory;
using SMT.Domain.Entities.Inventory;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Inventory
{
    using Microsoft.EntityFrameworkCore;

    public class PurchaseRepository(AppDbContext db) : BaseRepository<Purchase>(db), IPurchaseRepository
    {
        public async Task<PagedResult<PurchaseDto>> GetPagedAsync(SearchPurchaseDto dto)
        {
            var baseQuery = db.Purchases
                .Include(p => p.Vendor)
                .AsQueryable();

            // Filters
            if (!string.IsNullOrWhiteSpace(dto.SearchText))
            {
                baseQuery = baseQuery.Where(p =>
                    p.PurchaseNumber.Contains(dto.SearchText));
            }

            if (dto.StartDate.HasValue)
            {
                baseQuery = baseQuery.Where(p => p.PurchaseDate >= dto.StartDate.Value);
            }

            if (dto.EndDate.HasValue)
            {
                baseQuery = baseQuery.Where(p => p.PurchaseDate <= dto.EndDate.Value);
            }

            if (dto.VendorId.HasValue)
            {
                baseQuery = baseQuery.Where(p => p.VendorId == dto.VendorId.Value);
            }

            if (!string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                baseQuery = baseQuery.Where(p =>
                    p.Vendor.CompanyName.Contains(dto.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(dto.VendorName))
            {
                baseQuery = baseQuery.Where(p =>
                    p.Vendor.FirstName.Contains(dto.VendorName) ||
                    p.Vendor.LastName.Contains(dto.VendorName) ||
                    (p.Vendor.FirstName + " " + p.Vendor.LastName)
                        .Contains(dto.VendorName));
            }

            // Total count
            var totalCount = await baseQuery.CountAsync();

            int pageNumber = dto.PageNumber > 0 ? dto.PageNumber : 1;
            int pageSize = dto.PageSize > 0 ? dto.PageSize : 10;

            // Pagination FIRST
            var pagedData = await baseQuery
                .OrderByDescending(p => p.PurchaseDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // THEN projection (safe & clean)
            var items = pagedData.Select(p => new PurchaseDto
            {
                Id= p.Id,
                PurchaseNumber = p.PurchaseNumber,
                PurchaseDate = p.PurchaseDate,
                SubTotal = p.SubTotal,
                Discount = p.Discount,
                IsPaid = p.IsPaid,
                VendorId = p.VendorId,
                FirstName = p.Vendor.FirstName,
                LastName = p.Vendor.LastName,
                CompanyName = p.Vendor.CompanyName,

                // Get latest payment (or sum, depending on your logic)
                Amount = db.VendorPayments
                    .Where(vp => vp.PurchaseId == p.Id)
                    .Select(vp => (decimal?)vp.Amount)
                    .FirstOrDefault() ?? 0,

                PaymentMethod = db.VendorPayments
                    .Where(vp => vp.PurchaseId == p.Id)
                    .Select(vp => vp.PaymentMethod)
                    .FirstOrDefault()
            }).ToList();

            return new PagedResult<PurchaseDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = dto.PageNumber,
                PageSize = dto.PageSize
            };
        }

        public async Task UpdateSubTotalAsync(long purchaseId, decimal subTotal)
        {
            var purchase = await GetByIdAsync(purchaseId);
            if (purchase == null)
                throw new Exception($"Purchase not found: {purchaseId}");

            purchase.SubTotal = subTotal;
            await SaveChangesAsync();
        }
    }
}
