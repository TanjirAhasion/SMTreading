using SMT.Application.DTO.Inventory;
using SMT.Application.Helper;
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
        Task<SalesInvoiceDto> GetInvoiceByIdAsync(long id);
        Task<PagedResult<SalesDto>> GetPagedAsync(SearchSalesDto searchSalesDto);
    }
}
