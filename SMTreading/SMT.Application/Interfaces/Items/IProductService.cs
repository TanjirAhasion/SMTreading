using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.Interfaces.Items
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(long id);
        Task<long> CreateAsync(CreateProductDto dto);
        Task<long?> UpdateAsync(long id,UpdateProductDto dto);
        Task<bool> DeleteAsync(long id);
        Task<PagedResult<ProductListDto>> GetPagedAsync(int page, int pageSize, string? search, int? status);
    }
}
