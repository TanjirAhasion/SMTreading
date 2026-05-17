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
    public class CashAccountRepository(AppDbContext db) : BaseRepository<CashAccount>(db), ICashAccountRepository
    {
        public async Task<bool> UpdateCurrentBalance(long id, decimal currentBalance)
        {
            var cashAccount = await GetByIdAsync(id);
            if (cashAccount == null)
                return false;

            cashAccount.CurrentBalance = currentBalance;
            await  db.SaveChangesAsync();
            return true;
        }
    }
}
