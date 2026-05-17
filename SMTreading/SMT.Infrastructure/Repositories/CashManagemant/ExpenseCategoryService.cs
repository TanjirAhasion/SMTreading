using SMT.Application.DTO.CashManagement;
using SMT.Application.DTO.Items;
using SMT.Application.Interfaces.CashManagement;
using SMT.Application.Interfaces.Contacts;
using SMT.Domain.Entities.CashManagement;
using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.CashManagemant
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IExpenseCategoryRepository _repo;

        public ExpenseCategoryService(IExpenseCategoryRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<ExpenseCategoryDto>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();

            return categories.Select(x => new ExpenseCategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            }).ToList();
        }

        public async Task<ExpenseCategoryDto?> GetByIdAsync(long id)
        {
            var category = await _repo.GetByIdAsync(id);

            if (category == null) return null;
            return new ExpenseCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive
            };
        }

        public async Task<long> CreateAsync(ExpenseCategoryDto dto)
        {
            var category = new ExpenseCategory
            {
                Name = dto.Name
            };

            await _repo.CreateAsync(category);

            return category.Id;
        }

        public async Task<bool> UpdateAsync(long id, ExpenseCategoryDto dto)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return false;

            category.Name = dto.Name;
            category.IsActive = dto.IsActive;
            await _repo.UpdateAsync(category);

            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return false;

            await _repo.DeleteAsync(id);

            return true;
        }
    }
}
