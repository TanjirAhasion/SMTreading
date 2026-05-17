using SMT.Application.DTO.CashManagement;
using SMT.Application.DTO.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.CashManagement
{
    public interface IExpenseCategoryService
    {
        Task<List<ExpenseCategoryDto>> GetAllAsync();
        Task<ExpenseCategoryDto?> GetByIdAsync(long id);
        Task<long> CreateAsync(ExpenseCategoryDto dto);
        Task<bool> UpdateAsync(long id, ExpenseCategoryDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
