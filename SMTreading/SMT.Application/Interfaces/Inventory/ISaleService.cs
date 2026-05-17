using SMT.Application.DTO.Inventory;
using SMT.Application.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.Inventory
{
    public interface ISaleService
    {
        Task<long> CreateSaleAsync(CreateSaleRequest request);
        Task<SalesInvoiceDto> GetInvoiceByIdAsync(long id);
        Task<PagedResult<SalesDto>> GetPagedAsync(SearchSalesDto searchSalesDto);
    }
}
