using SMT.Application.DTO.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.Contacts
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdAsync(long id);
        Task<long> CreateAsync(CustomerDto dto);
        Task<bool> UpdateAsync(long id, CustomerDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
