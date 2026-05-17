using SMT.Application.DTO.CashManagement;
using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using SMT.Domain.Entities.CashManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.CashManagement
{
    public interface ICashTransactionService
    {
        Task<long> CreateAsync(CashTransactionDto dto);

        Task<List<CashTransactionDto>> GetAllAsync();

        Task<PagedResult<CashTransactionDto>> GetPagedAsync(int page, int pageSize, string? search, int? status);
    }
}
