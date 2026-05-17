using Microsoft.EntityFrameworkCore;
using SMT.Application.DTO.CashManagement;
using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using SMT.Application.Interfaces.CashManagement;
using SMT.Domain.Entities.CashManagement;
using SMT.Domain.Enums;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.CashManagemant
{
    public class CashTransactionRepository(AppDbContext db) : BaseRepository<CashTransaction>(db), ICashTransactionRepository
    {
        public async Task<PagedResult<CashTransactionDto>> GetPagedAsync(int page, int pageSize, string? search, int? status)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;
            search = search?.Trim();

            var baseQuery = db.CashTransactions
                .Include(p => p.CashAccount)
                .AsNoTracking()
                .Where(ps =>
                    string.IsNullOrEmpty(search) ||
                    EF.Functions.Like(ps.CashAccount.Name, $"%{search}%")
                );

            // Optional: filter only available stock
            //if (status != null)
            //    baseQuery = baseQuery.Where(ps => ps.Status == (ProductSerialStatus)status);

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new CashTransactionDto
                {
                    Id = p.Id,
                    CashAccountId = p.CashAccountId,
                    CashAccountName = p.CashAccount != null ? p.CashAccount.Name : "",
                    Amount = p.Amount,
                    TransactionType = (int)p.TransactionType,
                    SourceType = (int)p.SourceType,
                    ReferenceId = p.ReferenceId,
                    TransactionDate = p.TransactionDate,
                    Note = p.Note
                }).ToListAsync();
                    

            return new PagedResult<CashTransactionDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
