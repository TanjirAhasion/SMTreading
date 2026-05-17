using SMT.Application.DTO.CashManagement;
using SMT.Application.Interfaces.CashManagement;
using SMT.Domain.Entities.CashManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.CashManagemant
{
    public class CashAccountService : ICashAccountService
    {
        private readonly ICashAccountRepository _repo;

        public CashAccountService(ICashAccountRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<CashAccountDto>> GetAllAsync()
        {
            var accounts = await _repo.GetAllAsync();

            return accounts.Select(x => new CashAccountDto
            {
                Id = x.Id,
                Name = x.Name,
                AccountType = x.AccountType,
                MobileBankType = x.MobileBankType,
                AccountNumber = x.AccountNumber,
                AccountHolderName = x.AccountHolderName,
                BankName = x.BankName,
                BranchName = x.BranchName,
                Note = x.Note,
                OpeningBalance = x.OpeningBalance,
                CurrentBalance = x.CurrentBalance,
                IsDefault = x.IsDefault,
                IsActive = x.IsActive
            }).ToList();
        }

        public async Task<CashAccountDto?> GetByIdAsync(long id)
        {
            var account = await _repo.GetByIdAsync(id);

            if (account == null) return null;
            return new CashAccountDto
            {
                Id = account.Id,
                Name = account.Name,
                AccountType = account.AccountType,
                MobileBankType = account.MobileBankType,
                AccountNumber = account.AccountNumber,
                AccountHolderName = account.AccountHolderName,
                BankName = account.BankName,
                BranchName = account.BranchName,
                Note = account.Note,
                OpeningBalance = account.OpeningBalance,
                CurrentBalance = account.CurrentBalance,
                IsDefault = account.IsDefault,
                IsActive = account.IsActive
            };
        }

        public async Task<long> CreateAsync(CashAccountDto dto)
        {
            var account = new CashAccount
            {
                Name = dto.Name,
                AccountType = dto.AccountType,
                MobileBankType = dto.MobileBankType,
                AccountNumber = dto.AccountNumber,
                AccountHolderName = dto.AccountHolderName,
                BankName = dto.BankName,
                BranchName = dto.BranchName,
                Note = dto.Note,
                OpeningBalance = dto.OpeningBalance,
                CurrentBalance = dto.OpeningBalance,
                IsDefault = dto.IsDefault,
                IsActive = dto.IsActive
            };

            await _repo.CreateAsync(account);

            return account.Id;
        }

        public async Task<bool> UpdateAsync(long id, CashAccountDto dto)
        {
            var account = await _repo.GetByIdAsync(id);
            if (account == null) return false;

            account.Name = dto.Name;
            account.AccountType = dto.AccountType;
            account.MobileBankType = dto.MobileBankType;
            account.AccountNumber = dto.AccountNumber;
            account.AccountHolderName = dto.AccountHolderName;
            account.BankName = dto.BankName;
            account.BranchName = dto.BranchName;
            account.Note = dto.Note;
            account.OpeningBalance = dto.OpeningBalance;
            account.CurrentBalance = dto.CurrentBalance;
            account.IsDefault = dto.IsDefault;
            account.IsActive = dto.IsActive;
            await _repo.UpdateAsync(account);

            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var account = await _repo.GetByIdAsync(id);
            if (account == null) return false;

            await _repo.DeleteAsync(id);

            return true;
        }
    }
}
