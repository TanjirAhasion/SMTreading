using SMT.Application.DTO.Inventory;
using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.Inventory
{
    public interface IPurchaseService
    {
        Task<long> CreatePurchaseAsync(CreatePurchaseRequest dto);
        
        Task<PagedResult<PurchaseDto>> GetPagedAsync(SearchPurchaseDto searchPurchaseDto);
    }
}
