using SMT.Application.DTO.Items;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Repositories.Items
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _repo;

        public BrandService(IBrandRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<BrandDto>> GetAllAsync()
        {
            var brands = await _repo.GetAllAsync();

            return brands.Select(x => new BrandDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            }).ToList();
        }

        public async Task<BrandDto?> GetByIdAsync(long id)
        {
            var brand = await _repo.GetByIdAsync(id);

            if (brand == null) return null;

            return new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                IsActive = brand.IsActive
            };
        }

        public async Task<long> CreateAsync(BrandDto dto)
        {
            var brand = new Brand
            {
                Name = dto.Name
            };

            await _repo.CreateAsync(brand);

            return brand.Id;
        }

        public async Task<bool> UpdateAsync(long id, BrandDto dto)
        {
            var brand = await _repo.GetByIdAsync(id);
            if (brand == null) return false;

            brand.Name = dto.Name;
            brand.Description = dto.Description;
            brand.IsActive = dto.IsActive;

            await _repo.UpdateAsync(brand);

            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var brand = await _repo.GetByIdAsync(id);
            if (brand == null) return false;

            await _repo.DeleteAsync(id);

            return true;
        }
    }
}
