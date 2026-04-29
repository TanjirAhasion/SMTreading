using SMT.Application.DTO.Contacts;
using SMT.Application.Interfaces.Contacts;
using SMT.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Contacts
{
    public class CustomerService : ICustomerService 
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        // -------------------------------
        // CREATE
        // -------------------------------
        public async Task<long> CreateAsync(CustomerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                throw new ArgumentException("First name is required");

            var entity = new Customer
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
        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var customers = await _repo.GetAllAsync();

            return customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Phone = c.Phone,
                SecondaryPhone = c.SecondaryPhone,
                Email = c.Email,
                Address = c.Address,
                CompanyName = c.CompanyName,
                IsActive = c.IsActive
            }).ToList();
        }

        // -------------------------------
        // GET BY ID
        // -------------------------------
        public async Task<CustomerDto?> GetByIdAsync(long id)
        {
            var c = await _repo.GetByIdAsync(id);

            if (c == null) return null;

            return new CustomerDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Phone = c.Phone,
                SecondaryPhone = c.SecondaryPhone,
                Email = c.Email,
                Address = c.Address,
                CompanyName = c.CompanyName,
                IsActive = c.IsActive
            };
        }

        // -------------------------------
        // UPDATE
        // -------------------------------
        public async Task<bool> UpdateAsync(long id, CustomerDto dto)
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

            // 🔥 Soft delete
            entity.IsActive = false;

            await _repo.UpdateAsync(entity);

            return true;
        }
    }
}
