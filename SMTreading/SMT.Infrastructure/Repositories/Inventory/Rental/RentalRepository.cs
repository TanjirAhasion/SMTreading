using Humanizer;
using Microsoft.EntityFrameworkCore;
using SMT.Application.DTO.Inventory;
using SMT.Application.Helper;
using SMT.Application.Interfaces.Inventory.Rental;
using SMT.Domain.Entities.Inventory;
using SMT.Domain.Entities.Inventory.Rental;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Inventory.Rental
{
    public class RentalRepository(AppDbContext db) : BaseRepository<RentalInvoice>(db), IRentalRepository
    {
        public async Task<PagedResult<RentalDto>> GetPagedAsync(SearchRentalDto dto)
        {
            var baseQuery = db.RentalInvoices
                .Include(p => p.Customer)
                .AsQueryable();

            // Filters
            //if (!string.IsNullOrWhiteSpace(dto.SearchText))
            //{
            //    baseQuery = baseQuery.Where(p =>
            //        p.RentalNumber.Contains(dto.SearchText));
            //}

            //if (dto.StartDate.HasValue)
            //{
            //    baseQuery = baseQuery.Where(p => p.StartDate >= dto.StartDate.Value);
            //}

            //if (dto.EndDate.HasValue)
            //{
            //    baseQuery = baseQuery.Where(p => p.StartDate <= dto.EndDate.Value);
            //}

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
                .OrderByDescending(p => p.InvoiceDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // THEN projection (safe & clean)
            var items = pagedData.Select(p => new RentalDto
            {
                Id = p.Id,
                //RentalNumber = p.RentalNumber,
                RentalDate = p.InvoiceDate,
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

            return new PagedResult<RentalDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = dto.PageNumber,
                PageSize = dto.PageSize
            };
        }
    }
}
