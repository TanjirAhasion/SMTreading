using SMT.Application.Interfaces.Contacts;
using SMT.Domain.Entities.Contacts;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Contacts
{
    public class CustomerRepository(AppDbContext db) : BaseRepository<Customer>(db), ICustomerRepository
    {
    }
}
