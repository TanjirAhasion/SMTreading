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
    public class ExpenseRepository(AppDbContext db) : BaseRepository<Expense>(db), IExpenseRepository
    {
    }
}
