using SMT.Application.DTO.Contacts;
using SMT.Application.DTO.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.Contacts
{
    public interface IVendorService
    {
        Task<List<VendorDto>> GetAllAsync();
        Task<VendorDto?> GetByIdAsync(long id);
        Task<long> CreateAsync(VendorDto dto);
        Task<bool> UpdateAsync(long id, VendorDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
