using SMT.Application.DTO.CashManagement;
using SMT.Application.DTO.Items;
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
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repo;
        private readonly ICashTransactionService _cashTransactionService;
        public ExpenseService(IExpenseRepository repo, ICashTransactionService cashTransactionService)
        {
            _repo = repo;
            _cashTransactionService = cashTransactionService;
        }

        public async Task<long> CreateAsync(ExpenseDto dto)
        {
            var expense = new Expense
            {
                ExpenseCategoryId = dto.ExpenseCategoryId,
                Amount = dto.Amount,
                ExpenseDate = dto.ExpenseDate,
                CashAccountId = dto.CashAccountId,
                Note = dto.Note
            };

            await _repo.CreateAsync(expense);

            // IMPORTANT: CASH OUT ENTRY
            var cashTransaction = new CashTransactionDto
            {
                CashAccountId = dto.CashAccountId,
                Amount = dto.Amount,

                TransactionType = (int)TransactionType.CashOut, // CashOut
                SourceType = (int)TransactionSource.Expense, // Expense

                ReferenceId = expense.Id,
                TransactionDate = dto.ExpenseDate,
                Note = dto.Note
            };

            await _cashTransactionService.CreateAsync(cashTransaction);
            return expense.Id;
        }

        public async Task<List<ExpenseDto>> GetAllAsync()
        {
            var expenses = await _repo.GetAllAsync();

            return expenses.Select(x => new ExpenseDto
            {
                Id = x.Id,
                ExpenseCategoryId = x.ExpenseCategoryId,
                Amount = x.Amount,
                ExpenseDate = x.ExpenseDate,
                CashAccountId = x.CashAccountId ?? 0,
                Note = x.Note
            }).ToList();
        }

        public async Task<ExpenseDto?> GetByIdAsync(long id)
        {
            var expense = await _repo.GetByIdAsync(id);

            if (expense == null) return null;

            return new ExpenseDto
            {
                Id = expense.Id,
                ExpenseCategoryId = expense.ExpenseCategoryId,
                Amount = expense.Amount,
                ExpenseDate = expense.ExpenseDate,
                CashAccountId = expense.CashAccountId ?? 0,
                Note = expense.Note
            };
        }
    }
}
