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
    public class VendorPaymentRepository(AppDbContext db) : BaseRepository<VendorPayment>(db), IVendorPaymentRepository
    {
    }
}
