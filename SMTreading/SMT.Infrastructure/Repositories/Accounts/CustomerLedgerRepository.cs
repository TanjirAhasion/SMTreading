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
    public class CustomerLedgerRepository(AppDbContext db) : BaseRepository<CustomerLedger>(db), ICustomerLedgerRepository
    {
        public async Task<long> CreateCustomerLedger(CustomerLedgerDto dto)
        {
            var lastEntry = db.Set<CustomerLedger>()
                         .Where(x => x.CustomerId == dto.CustomerId)
                         .OrderByDescending(x => x.Id)
                         .FirstOrDefault();
            decimal previousBalance = 0;

            if (lastEntry != null)
                previousBalance = lastEntry?.Balance ?? 0;

            // 2. Calculate new balance
            decimal newBalance = previousBalance + dto.Debit - dto.Credit;

            var entity = new CustomerLedger
            {
                CustomerId = dto.CustomerId,
                SourceType = dto.SourceType,
                SourceId = dto.SourceId,
                Credit = dto.Credit,
                Debit = dto.Debit,
                Balance = newBalance,
                Description = dto.Description
            };

            await db.Set<CustomerLedger>().AddAsync(entity);
            await db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<CustomerLedger?> GetLastEntryByCustomerAsync(long customerId)
        {
            return await db.Set<CustomerLedger>()
                     .Where(x => x.CustomerId == customerId)
                     .OrderByDescending(x => x.Id)
                     .FirstOrDefaultAsync();
        }
    }
}
