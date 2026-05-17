using SMT.Application.DTO.Inventory;
using SMT.Application.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.Inventory.Rental
{
    public interface IRentalContractService
    {
        Task<long> CreateAsync(CreateRentalContractRequest request);

        Task CloseAsync(long contractId);
        
        Task<PagedResult<RentalContractDto>> GetPagedAsync(SearchRentalContractDto searchSaleDto);
        Task<RentalContractInvoiceDto> GetRentalContractByIdAsync(long id);
    }
}
