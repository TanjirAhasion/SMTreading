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
    public interface IPurchaseRepository : IBaseRepository<Purchase>
    {
        Task<PagedResult<PurchaseDto>> GetPagedAsync(SearchPurchaseDto searchPurchaseDto);
       
        Task UpdateSubTotalAsync(long purchaseId, decimal subTotal);
    }
}
