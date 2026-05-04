using SMT.Application.DTO.Items;
using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.Interfaces.Items
{
    public interface IProductImageService
    {
        Task<List<ProductImageDto>> GetAllAsync();
        Task<ProductImageDto?> GetByIdAsync(long id);
        Task<ProductImageDto> CreateAsync([Microsoft.AspNetCore.Mvc.FromForm] CreateProductImageDto dto);
        Task<ProductImageDto?> UpdateAsync(long id, UpdateProductImageDto dto);
        Task<bool> DeleteAsync(long id);
        Task<List<ProductImageWithLinkedDto>> GetBySerialIdAsync(long serialId);
    }
}
