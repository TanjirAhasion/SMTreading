using SMT.Application.DTO.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.Accounts
{
    public interface IVendorPaymentService
    {
        Task<List<VendorPaymentDto>> GetAllAsync();
        Task<VendorPaymentDto?> GetByIdAsync(long id);
        Task<long> CreateAsync(VendorPaymentDto dto);
    }
}
