using SMT.Application.DTO.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.Interfaces.Items
{
    public interface IBrandService
    {
        Task<List<BrandDto>> GetAllAsync();
        Task<BrandDto?> GetByIdAsync(long id);
        Task<long> CreateAsync(BrandDto dto);
        Task<bool> UpdateAsync(long id, BrandDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
