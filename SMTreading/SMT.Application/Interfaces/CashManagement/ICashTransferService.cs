using SMT.Application.DTO.CashManagement;
using SMT.Application.Helper;
using SMT.Domain.Entities.CashManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.CashManagement
{
    public interface ICashTransferService
    {
        Task<long> CreateAsync(CashTransferDto dto);
        Task<List<CashTransferDto>> GetAllAsync();
        Task<long> TransferAsync(CashTransferDto dto);
        Task<PagedResult<CashTransferDto>> GetPagedAsync(int page, int pageSize, string? search);
    }
}
