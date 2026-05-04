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
    }
}
