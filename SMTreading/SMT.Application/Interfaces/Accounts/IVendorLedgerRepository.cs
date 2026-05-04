using SMT.Application.DTO.Accounts;
using SMT.Domain.Entities.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.Accounts
{
    public interface IVendorLedgerRepository : IBaseRepository<VendorLedger>
    {
        Task<VendorLedger?> GetLastEntryByVendorAsync(long vendorId);

        Task<long> CreateVendorLedger(VendorLedgerDto dto);
    }
}
