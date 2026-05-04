using SMT.Application.DTO.Accounts;
using SMT.Application.DTO.Items;
using SMT.Application.Interfaces.Accounts;
using SMT.Domain.Entities.Accounts;
using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Accounts
{
    public class VendorLedgerService : IVendorLedgerService
    {
        private readonly IVendorLedgerRepository _repo;
        public VendorLedgerService(IVendorLedgerRepository repo)
        {
            _repo = repo;
        }   

        public async Task<long> CreateAsync(VendorLedgerDto dto)
        {
            var entity = await _repo.CreateAsync(new VendorLedger
            {
                VendorId = dto.VendorId,
                SourceType = dto.SourceType,
                SourceId = dto.SourceId,
                Credit = dto.Credit,
                Debit = dto.Debit,
                Balance = dto.Balance
            });

            return entity.Id;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<List<VendorLedgerDto>> GetAllAsync()
        {
            var result = await _repo.GetAllAsync();
            
            return result.Select(x => new VendorLedgerDto
            {
                Id = x.Id,
                VendorId = x.VendorId,
                SourceType = x.SourceType,
                SourceId= x.SourceId,
                Credit = x.Credit,
                Debit = x.Debit,
                Balance = x.Balance
            }).ToList();
        }

        public async Task<VendorLedgerDto?> GetByIdAsync(long id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            return new VendorLedgerDto
            {
                Id = entity.Id,
                VendorId = entity.VendorId,
                SourceType = entity.SourceType,
                SourceId = entity.SourceId,
                Credit = entity.Credit,
                Debit = entity.Debit,
                Balance = entity.Balance
            };
        }
    }
}
