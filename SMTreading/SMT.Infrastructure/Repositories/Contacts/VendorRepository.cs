using SMT.Application.Interfaces.Contacts;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using StockoraPOS.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Contacts
{
    public class VendorRepository(AppDbContext db) : BaseRepository<Vendor>(db), IVendorRepository
    {
    }
}
