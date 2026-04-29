using SMT.Application.DTO.Contacts;
using SMT.Application.Interfaces.Contacts;
using StockoraPOS.Domain.Entities.Contacts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Contacts
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _repo;

        public VendorService(IVendorRepository repo)
        {
            _repo = repo;
        }

        // -------------------------------
        // CREATE
        // -------------------------------
        public async Task<long> CreateAsync(VendorDto dto)
        {
            var entity = new Vendor
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName ?? string.Empty,
                Phone = dto.Phone,
                SecondaryPhone = dto.SecondaryPhone,
                Email = dto.Email,
                Address = dto.Address,
                CompanyName = dto.CompanyName,
                IsActive = true
            };

            await _repo.CreateAsync(entity);
            return entity.Id;
        }

        // -------------------------------
        // GET ALL
        // -------------------------------
        public async Task<List<VendorDto>> GetAllAsync()
        {
            var vendors = await _repo.GetAllAsync();

            return vendors.Select(v => new VendorDto
            {
                Id = v.Id,
                FirstName = v.FirstName,
                LastName = v.LastName,
                Phone = v.Phone,
                SecondaryPhone = v.SecondaryPhone,
                Email = v.Email,
                Address = v.Address,
                CompanyName = v.CompanyName,
                IsActive = v.IsActive
            }).ToList();
        }

        // -------------------------------
        // GET BY ID
        // -------------------------------
        public async Task<VendorDto?> GetByIdAsync(long id)
        {
            var v = await _repo.GetByIdAsync(id);

            if (v == null) return null;

            return new VendorDto
            {
                Id = v.Id,
                FirstName = v.FirstName,
                LastName = v.LastName,
                Phone = v.Phone,
                SecondaryPhone = v.SecondaryPhone,
                Email = v.Email,
                Address = v.Address,
                CompanyName = v.CompanyName,
                IsActive = v.IsActive
            };
        }

        // -------------------------------
        // UPDATE
        // -------------------------------
        public async Task<bool> UpdateAsync(long id, VendorDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);

            if (entity == null) return false;

            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName ?? string.Empty;
            entity.Phone = dto.Phone;
            entity.SecondaryPhone = dto.SecondaryPhone;
            entity.Email = dto.Email;
            entity.Address = dto.Address;
            entity.CompanyName = dto.CompanyName;
            entity.IsActive = dto.IsActive;

            await _repo.UpdateAsync(entity);

            return true;
        }

        // -------------------------------
        // DELETE (Soft Delete)
        // -------------------------------
        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _repo.GetByIdAsync(id);

            if (entity == null) return false;

            // 🔥 Soft delete (recommended)
            entity.IsActive = false;

            await _repo.UpdateAsync(entity);
            return true;
        }
    }
}
