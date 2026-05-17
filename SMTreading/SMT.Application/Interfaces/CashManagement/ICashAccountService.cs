using SMT.Application.DTO.CashManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.CashManagement
{
    public interface ICashAccountService
    {
        Task<List<CashAccountDto>> GetAllAsync();
        Task<CashAccountDto?> GetByIdAsync(long id);
        Task<long> CreateAsync(CashAccountDto dto);
        Task<bool> UpdateAsync(long id, CashAccountDto dto);
        Task<bool> DeleteAsync(long id);
       

    }
}
