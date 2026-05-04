using Microsoft.EntityFrameworkCore;
using SMT.Application.DTO.Accounts;
using SMT.Application.Interfaces.Accounts;
using SMT.Domain.Entities.Accounts;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Accounts
{
    public class VendorLedgerRepository(AppDbContext db) : BaseRepository<VendorLedger>(db), IVendorLedgerRepository
    {
        public async Task<long> CreateVendorLedger(VendorLedgerDto dto)
        {
            var lastEntry = db.Set<VendorLedger>()
                          .Where(x => x.VendorId == dto.VendorId)
                          .OrderByDescending(x => x.Id)
                          .FirstOrDefault();
            decimal previousBalance = 0;

            if (lastEntry != null)
                previousBalance = lastEntry?.Balance ?? 0;

            // 2. Calculate new balance
            decimal newBalance = previousBalance + dto.Debit - dto.Credit;

            var entity = new VendorLedger
            {
                VendorId = dto.VendorId,
                SourceType = dto.SourceType,
                SourceId = dto.SourceId,
                Credit = dto.Credit,
                Debit = dto.Debit,
                Balance = newBalance
            };

            await db.Set<VendorLedger>().AddAsync(entity);
            await db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<VendorLedger?> GetLastEntryByVendorAsync(long vendorId)
        {
            return await db.Set<VendorLedger>()
                            .Where(x => x.VendorId == vendorId)
                            .OrderByDescending(x => x.Id)
                            .FirstOrDefaultAsync();
        }
    }
}
