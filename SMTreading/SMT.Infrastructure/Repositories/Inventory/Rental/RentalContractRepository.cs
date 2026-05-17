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
    public class RentalContractRepository(AppDbContext db) : BaseRepository<RentalContract>(db), IRentalContractRepository
    {
        public async Task<PagedResult<RentalContractDto>> GetPagedAsync(SearchRentalContractDto dto)
        {
            var baseQuery = db.RentalContracts
                .Include(p => p.Customer)
                .AsQueryable();

            // Filters
            if (!string.IsNullOrWhiteSpace(dto.SearchText))
            {
                baseQuery = baseQuery.Where(p =>
                    p.ContractNumber.Contains(dto.SearchText));
            }

            if (dto.StartDate.HasValue)
            {
                baseQuery = baseQuery.Where(p => p.StartDate >= dto.StartDate.Value);
            }

            if (dto.EndDate.HasValue)
            {
                baseQuery = baseQuery.Where(p => p.EndDate <= dto.EndDate.Value);
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
                .OrderByDescending(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // THEN projection (safe & clean)
            var items = pagedData.Select(p => new RentalContractDto
            {
                Id = p.Id,
                ContractNumber = p.ContractNumber,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                BillingCycle = (int)p.BillingCycle,
                NextBillingDate = p.NextBillingDate,
                Status = (int)p.Status,
                Note = p.Note,
                CustomerId = p.CustomerId,
                FirstName = p.Customer.FirstName,
                LastName = p.Customer.LastName,
                CompanyName = p.Customer.CompanyName,

                PaymentMethod = db.CustomerPayments
                    .Where(cp => cp.SaleId == p.Id)
                    .Select(cp => cp.PaymentMethod)
                    .FirstOrDefault()
            }).ToList();

            return new PagedResult<RentalContractDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = dto.PageNumber,
                PageSize = dto.PageSize
            };
        }

        public async Task<RentalContractInvoiceDto?> GetRentalContractByIdAsync(long id)
        {
            var contract = await db.RentalContracts
                .Include(c => c.Customer)
                .Include(c => c.Items)
                    .ThenInclude(i => i.ProductSerial)
                        .ThenInclude(s => s.Product)
                            .ThenInclude(p => p.Brand)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
                return null;

            var groupedItems = contract.Items?
                .Where(i => i.ProductSerial != null && i.ProductSerial.Product != null)
                .GroupBy(i => i.ProductSerial.ProductId)
                .Select(g => new RentalContractInvoiceItemDto
                {
                    ProductName = g.First().ProductSerial.Product?.Name ?? "",
                    Model = g.First().ProductSerial.Product?.Model ?? "",
                    BrandName = g.First().ProductSerial.Product?.Brand?.Name ?? "",
                    Quantity = g.Count(),
                    UnitRent = g.Average(x => x.Rate),
                    TotalRent = g.Sum(x => x.Rate),
                    SerialNumbers = g
                        .Where(x => x.ProductSerial != null)
                        .Select(x => x.ProductSerial.SerialNumber)
                        .ToList()
                })
                .ToList() ?? new List<RentalContractInvoiceItemDto>();

            return new RentalContractInvoiceDto
            {
                Id = contract.Id,
                ContractNumber = contract.ContractNumber,
                StartDate = contract.StartDate,
                SecurityDeposit = contract.SecurityDeposit ?? 0,

                CustomerFullName =
                    $"{contract.Customer?.FirstName} {contract.Customer?.LastName}",

                CompanyName = contract.Customer?.CompanyName ?? "",

                InvoiceItems = groupedItems
            };
        }
    }
}