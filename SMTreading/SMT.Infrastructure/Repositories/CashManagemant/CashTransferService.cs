using Humanizer;
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
    public class CashTransferService : ICashTransferService
    {
        private readonly ICashTransferRepository _repo;
        private readonly ICashTransactionRepository _transactionRepo;

        public CashTransferService(ICashTransferRepository repo, ICashTransactionRepository transactionRepo)
        {
            _repo = repo;
            _transactionRepo = transactionRepo;
        }

        public async Task<long> CreateAsync(CashTransferDto dto)
        {
            var cashTransfer = new CashTransfer
            {
                FromCashAccountId = dto.FromCashAccountId,
                ToCashAccountId = dto.ToCashAccountId,

                Amount = dto.Amount,
                TransferDate = dto.TransferDate,
                Note = dto.Note
            };

            await _repo.CreateAsync(cashTransfer);

            return cashTransfer.Id;
        }

        public async Task<List<CashTransferDto>> GetAllAsync()
        {
            var cashTransfers = await _repo.GetAllAsync();

            return cashTransfers.Select(x => new CashTransferDto
            {
                Id = x.Id,

                FromCashAccountId = x.FromCashAccountId,
                ToCashAccountId = x.ToCashAccountId,

                Amount = x.Amount,
                TransferDate = x.TransferDate,
                Note = x.Note
            }).ToList();
        }

        public async Task<PagedResult<CashTransferDto>> GetPagedAsync(int page, int pageSize, string? search)
        {
            return await _repo.GetPagedAsync(page, pageSize, search);
        }

        public async Task<long> TransferAsync(CashTransferDto dto)
        {
            await _repo.BeginTransactionAsync();

            try
            {
                // 1. Save transfer header
                var transfer = new CashTransfer
                {
                    FromCashAccountId = dto.FromCashAccountId,
                    ToCashAccountId = dto.ToCashAccountId,

                    Amount = dto.Amount,
                    TransferDate = dto.TransferDate,
                    Note = dto.Note
                };

                await _repo.CreateAsync(transfer);

                // 2. Cash OUT
                await _transactionRepo.CreateAsync(new CashTransaction
                {
                    CashAccountId = dto.FromCashAccountId,
                    Amount = dto.Amount,
                    TransactionType = TransactionType.CashOut, // Out
                    SourceType = TransactionSource.CashTransfer, // Transfer
                    ReferenceId = transfer.Id,
                    TransactionDate = dto.TransferDate,
                    Note = "Transfer Out"
                });

                // 3. Cash IN
                await _transactionRepo.CreateAsync(new CashTransaction
                {
                    CashAccountId = dto.ToCashAccountId,
                    Amount = dto.Amount,
                    TransactionType = TransactionType.CashIn, // In
                    SourceType = TransactionSource.CashTransfer, // Transfer
                    ReferenceId = transfer.Id,
                    TransactionDate = dto.TransferDate,
                    Note = "Transfer In"
                });

                await _repo.CommitTransactionAsync();

                return transfer.Id;
            }
            catch
            {
                await _repo.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
