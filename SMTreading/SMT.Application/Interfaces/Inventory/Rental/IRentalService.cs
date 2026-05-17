using SMT.Application.DTO.Inventory;
using SMT.Application.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.Inventory.Rental
{
    public interface IRentalService
    {
        Task<long> CreateRentalAsync(CreateRentalRequest request);

        Task<PagedResult<RentalDto>> GetPagedAsync(SearchRentalDto searchRentalDto);
    }
}
