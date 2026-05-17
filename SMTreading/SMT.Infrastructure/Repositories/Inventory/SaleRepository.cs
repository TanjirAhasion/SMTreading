using Microsoft.EntityFrameworkCore;
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
    public class SaleRepository(AppDbContext db) : BaseRepository<SaleInvoice>(db), ISaleRepository
    {
        public async Task<SalesInvoiceDto> GetInvoiceByIdAsync(long id)
        {
            var saleInvoice = await db.SaleInvoices
              .Include(p => p.Customer)
              .Include(p => p.Items)
                  .ThenInclude(i => i.Product)
                      .ThenInclude(p => p.Brand)
              .FirstOrDefaultAsync(p => p.Id == id);

            if (saleInvoice == null)
                return null;

            var invoice = new SalesInvoiceDto
            {
                Id = saleInvoice.Id,
                InvoiceNumber = saleInvoice.InvoiceNumber,
                SaleDate = saleInvoice.SaleDate,
                SubTotal = saleInvoice.SubTotal,
                Discount = saleInvoice.Discount,
                PaidAmount = saleInvoice.PaidAmount ?? 0,
                CustomerFullName = $"{saleInvoice.Customer.FirstName} {saleInvoice.Customer.LastName}",
                CompanyName = saleInvoice.Customer.CompanyName ?? "",
                InvoiceItems = saleInvoice.Items.Select(pi => new SalesInvoiceItemDto
                {
                    Id = pi.Id,
                    ProductName = pi.Product?.Name ?? "",
                    Model = pi.Product?.Model ?? "",
                    BrandName = pi.Product?.Brand?.Name ?? "",
                    Quantity = pi.Quantity,
                    UnitPrice = pi.UnitPrice
                }).ToList()
            };

            return invoice;

        }

        public async Task<PagedResult<SalesDto>> GetPagedAsync(SearchSalesDto dto)
        {
            var baseQuery = db.SaleInvoices
                .Include(p => p.Customer)
                .AsQueryable();

            // Filters
            if (!string.IsNullOrWhiteSpace(dto.SearchText))
            {
                baseQuery = baseQuery.Where(p =>
                    p.InvoiceNumber.Contains(dto.SearchText));
            }

            if (dto.StartDate.HasValue)
            {
                baseQuery = baseQuery.Where(p => p.SaleDate >= dto.StartDate.Value);
            }

            if (dto.EndDate.HasValue)
            {
                baseQuery = baseQuery.Where(p => p.SaleDate <= dto.EndDate.Value);
            }

            if (dto.CustomerId.HasValue)
            {
                baseQuery = baseQuery.Where(p => p.CustomerId == dto.CustomerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                baseQuery = baseQuery.Where(p =>
                    p.Customer.CompanyName.Contains(dto.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(dto.CustomerName))
            {
                baseQuery = baseQuery.Where(p =>
                    p.Customer.FirstName.Contains(dto.CustomerName) ||
                    p.Customer.LastName.Contains(dto.CustomerName) ||
                    (p.Customer.FirstName + " " + p.Customer.LastName)
                        .Contains(dto.CustomerName));
            }

            // Total count
            var totalCount = await baseQuery.CountAsync();

            int pageNumber = dto.PageNumber > 0 ? dto.PageNumber : 1;
            int pageSize = dto.PageSize > 0 ? dto.PageSize : 10;

            // Pagination FIRST
            var pagedData = await baseQuery
                .OrderByDescending(p => p.SaleDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // THEN projection (safe & clean)
            var items = pagedData.Select(p => new SalesDto
            {
                Id = p.Id,
                InvoiceNumber = p.InvoiceNumber,
                SaleDate = p.SaleDate,
                SubTotal = p.SubTotal,
                Discount = p.Discount,
                IsPaid = p.IsPaid,
                CustomerId = p.CustomerId,
                FirstName = p.Customer.FirstName,
                LastName = p.Customer.LastName,
                CompanyName = p.Customer.CompanyName,

                // Get latest payment (or sum, depending on your logic)
                PaidAmount = db.CustomerPayments
                    .Where(cp => cp.SaleId == p.Id)
                    .Select(cp => (decimal?)cp.Amount)
                    .FirstOrDefault() ?? 0,

                PaymentMethod = db.CustomerPayments
                    .Where(cp => cp.SaleId == p.Id)
                    .Select(cp => cp.PaymentMethod)
                    .FirstOrDefault()
            }).ToList();

            return new PagedResult<SalesDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = dto.PageNumber,
                PageSize = dto.PageSize
            };
        }
    }
}
