using SMT.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.Inventory
{
    public interface ISaleRepository : IBaseRepository<SaleInvoice>
    {
    }
}
