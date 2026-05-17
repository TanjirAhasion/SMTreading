using SMT.Application.DTO.CashManagement;
using SMT.Application.DTO.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.CashManagement
{
    public interface IExpenseService
    {
        public Task<long> CreateAsync(ExpenseDto dto);
        Task<List<ExpenseDto>> GetAllAsync();
        Task<ExpenseDto?> GetByIdAsync(long id);
    }
}
