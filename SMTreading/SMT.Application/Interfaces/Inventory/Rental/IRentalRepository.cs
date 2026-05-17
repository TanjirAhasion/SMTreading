using SMT.Application.DTO.Inventory;
using SMT.Application.Helper;
using SMT.Domain.Entities.Inventory.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.Inventory.Rental
{
    public interface IRentalRepository : IBaseRepository<RentalInvoice>
    {
        Task<PagedResult<RentalDto>> GetPagedAsync(SearchRentalDto searchRentalDto);
    }
}
