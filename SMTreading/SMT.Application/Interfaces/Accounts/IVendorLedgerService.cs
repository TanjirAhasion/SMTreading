using SMT.Application.DTO.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.Accounts
{
    public interface IVendorLedgerService
    {
        Task<List<VendorLedgerDto>> GetAllAsync();
        Task<VendorLedgerDto?> GetByIdAsync(long id);
        Task<long> CreateAsync(VendorLedgerDto dto);
        //Task<bool> UpdateAsync(long id, VendorLedgerDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
