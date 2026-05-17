using SMT.Application.DTO.CashManagement;
using SMT.Application.Helper;
using SMT.Application.Interfaces.CashManagement;
using SMT.Domain.Entities;
using SMT.Domain.Entities.CashManagement;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.CashManagemant
{
    public class CashTransactionService : ICashTransactionService
    {
        private readonly ICashTransactionRepository _repo;
        private readonly ICashAccountRepository _cashAccountRepo;
        public CashTransactionService(ICashTransactionRepository repo, ICashAccountRepository cashAccountRepo)
        {
            _repo = repo;
            _cashAccountRepo = cashAccountRepo;
        }

        public async Task<long> CreateAsync(CashTransactionDto dto)
        {
            var cashAccount = await _cashAccountRepo
         .GetByIdAsync(dto.CashAccountId);

            if (cashAccount == null)
                throw new Exception("Cash account not found");

            // Balance validation
            if (dto.TransactionType == (int)TransactionType.CashOut)
            {
                if (cashAccount.CurrentBalance < dto.Amount)
                    throw new Exception("Insufficient balance");

                cashAccount.CurrentBalance -= dto.Amount;
            }
            else
            {
                cashAccount.CurrentBalance += dto.Amount;
            }

            await _cashAccountRepo.UpdateAsync(cashAccount);

            var transaction = new CashTransaction
            {
                CashAccountId = dto.CashAccountId,
                Amount = dto.Amount,
                TransactionType = (TransactionType)dto.TransactionType,
                SourceType = (TransactionSource)dto.SourceType,
                ReferenceId = dto.ReferenceId,
                TransactionDate = dto.TransactionDate,
                Note = dto.Note
            };

            await _repo.CreateAsync(transaction);

            return transaction.Id;
        }

        public async Task<List<CashTransactionDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();

            return list.Select(x => new CashTransactionDto
            {
                CashAccountId = x.CashAccountId,
                Amount = x.Amount,
                TransactionType = (int)x.TransactionType,
                SourceType = (int)x.SourceType,
                ReferenceId = x.ReferenceId,
                TransactionDate = x.TransactionDate,
                Note = x.Note
            }).ToList();
        }

        public Task<PagedResult<CashTransactionDto>> GetPagedAsync(int page, int pageSize, string? search, int? status)
        {
            return _repo.GetPagedAsync(page, pageSize, search, status);
        }
    }
}
