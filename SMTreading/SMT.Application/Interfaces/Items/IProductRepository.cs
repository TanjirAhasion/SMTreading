using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.Interfaces.Items
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<List<Product>> GetAllWithBrandsAsync();
        Task<List<Product>> GetByIdsAsync(List<long> productIds);
        Task<PagedResult<ProductListDto>> GetPagedAsync(int page, int pageSize, string? search, int? status);
    }
}
