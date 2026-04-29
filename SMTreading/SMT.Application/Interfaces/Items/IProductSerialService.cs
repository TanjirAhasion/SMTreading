using SMT.Application.DTO.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.Interfaces.Items
{
    public interface IProductSerialService
    {
        Task<List<ProductSerialDto>> GetAllAsync(); 
        Task<ProductSerialDto?> GetByIdAsync(long id); 
        Task<ProductSerialDto> CreateAsync(CreateProductSerialDto dto); 
        Task<long?> UpdateAsync(long id, UpdateProductSerialDto dto); 
        Task<bool> DeleteAsync(long id);
    }
}
