using Microsoft.EntityFrameworkCore;
using SMT.Application.DTO.CashManagement;
using SMT.Application.Helper;
using SMT.Application.Interfaces.CashManagement;
using SMT.Domain.Entities.CashManagement;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.CashManagemant
{
    public class CashTransferRepository(AppDbContext db) : BaseRepository<CashTransfer>(db), ICashTransferRepository
    {
        public async Task<PagedResult<CashTransferDto>> GetPagedAsync(int page, int pageSize, string? search)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;
            search = search?.Trim();

            var baseQuery = db.CashTransfers
                .Include(p => p.FromAccount)
                .Include(p => p.ToAccount)
                .AsNoTracking()
                .Where(ps =>
                    string.IsNullOrEmpty(search) ||
                    EF.Functions.Like(ps.ToAccount.Name, $"%{search}%") ||
                    EF.Functions.Like(ps.FromAccount.Name, $"%{search}%")
                );

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new CashTransferDto
                {
                    Id = p.Id,
                    FromCashAccountId = p.FromCashAccountId,
                    ToCashAccountId = p.ToCashAccountId,
                    Amount = p.Amount,
                    Note = p.Note
                }).ToListAsync();


            return new PagedResult<CashTransferDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
